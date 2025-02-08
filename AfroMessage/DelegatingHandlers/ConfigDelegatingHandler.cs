using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Web;

namespace AfroMessage.DelegatingHandlers;

public class ConfigDelegatingHandler : DelegatingHandler
{
    private readonly AfroMessageConfig config;

    public ConfigDelegatingHandler(AfroMessageConfig config)
    {
        this.config = config;
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Console.WriteLine("endpoint: " + request.RequestUri?.ToString());
        if (request.Method == HttpMethod.Post)
        {
            var body = await request.Content?.ReadFromJsonAsync<JsonNode>(cancellationToken)!;
            body!["from"] = config.Identifier;
            body["sender"] = config.Sender;
            request.Content = JsonContent.Create(body);

            Console.WriteLine("request body: " + body);
        }
        else if (request.Method == HttpMethod.Get)
        {
            var uriBuilder = new UriBuilder(request.RequestUri!);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            if (!string.IsNullOrWhiteSpace(config.Identifier))
                query["from"] = config.Identifier;
            if (!string.IsNullOrWhiteSpace(config.Sender))
                query["sender"] = config.Sender;

            uriBuilder.Query = query.ToString();
            request.RequestUri = uriBuilder.Uri;
            Console.WriteLine("request url: " + uriBuilder.Uri.ToString());
        }
        Console.WriteLine(request.ToString());

        return await base.SendAsync(request, cancellationToken);
    }

}
