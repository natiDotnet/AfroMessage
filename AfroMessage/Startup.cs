using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using AfroMessage.DelegatingHandlers;

namespace AfroMessage;

public static class Startup
{
    public static IServiceCollection AddAfroMessage(this IServiceCollection services, AfroMessageConfig? config = null)
    {
        services
        .AddScoped(sp =>
        {
            config ??= sp.GetRequiredService<IOptions<AfroMessageConfig>>().Value;
            return new ConfigDelegatingHandler(config);
        })
        .AddScoped<RetryDelegatingHandler>();
        services.AddOptions<AfroMessageConfig>().BindConfiguration(AfroMessageConfig.Key);
        services
            .AddHttpClient<IAfroMessageClient>()
            .AddTypedClient((client, sp) =>
            {
                config ??= sp.GetRequiredService<IOptions<AfroMessageConfig>>().Value;
                client.BaseAddress = new Uri(AfroMessageConfig.Url);
                //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.Token}");
                return new AfroMessageClient(config, client);
            })            
            .AddHttpMessageHandler<ConfigDelegatingHandler>()
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new SocketsHttpHandler()
                {
                    PooledConnectionLifetime = TimeSpan.FromMinutes(15)
                };
            })
            .SetHandlerLifetime(Timeout.InfiniteTimeSpan)
            .AddStandardResilienceHandler();

        return services;
    }

}
