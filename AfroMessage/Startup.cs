using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using AfroMessage;
using Microsoft.Extensions.Options;
using AfroMessage.DelegatingHandlers;
using System.Net.Http;
using System.Threading;
namespace AfroMessage;

public static class Startup
{
    public static IServiceCollection AddAfroMessage(this IServiceCollection services, AfroMessageConfig? config = null)
    {
        services
         .AddScoped<ConfigDelegatingHandler>(sp =>
         {
             config ??= sp.GetRequiredService<IOptions<AfroMessageConfig>>().Value;
             return new ConfigDelegatingHandler(config);
         })
         .AddScoped<RetryDelegatingHandler>();
        services.AddOptions<AfroMessageConfig>().BindConfiguration(AfroMessageConfig.Key);
        services
            .AddHttpClient<IAfroMessage>()
            .AddHttpMessageHandler<ConfigDelegatingHandler>()
            .AddHttpMessageHandler<RetryDelegatingHandler>()
            .AddTypedClient((client, sp) =>
            {
                config ??= sp.GetRequiredService<IOptions<AfroMessageConfig>>().Value;
                client.BaseAddress = new Uri(AfroMessageConfig.Url);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.Token}");
                return new AfroMessageClient(config, client);
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new SocketsHttpHandler()
                {
                    PooledConnectionLifetime = TimeSpan.FromMinutes(15)
                };
            })
            .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

        return services;
    }

}
