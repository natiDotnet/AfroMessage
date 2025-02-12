using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using AfroMessage.DelegatingHandlers;
using AfroMessage.Requests;
using AfroMessage.Responses;
using AfroMessage.Results;
using System.Net.Http.Headers;
using Microsoft.Extensions.Diagnostics.Latency;
using System.Diagnostics;

namespace AfroMessage;

public class AfroMessageClient : IAfroMessageClient
{
    private readonly HttpClient client;
    private readonly AfroMessageConfig config;
    public AfroMessageClient(AfroMessageConfig config, HttpClient? client = null)
    {
        client ??= new HttpClient(new ConfigDelegatingHandler(config)
        {
            InnerHandler = new RetryDelegatingHandler()
        })
        {
            BaseAddress = new Uri(AfroMessageConfig.Url)
        };
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", config.Token);
        this.config = config;
        this.client = client;
    }
    public AfroMessageClient(string token, string identifier, string sender) 
        : this( new AfroMessageConfig { Token = token, Identifier = identifier, Sender = sender})
    { }

    private async Task<Result<T>> HandleApiResponse<T>(HttpResponseMessage response)
    {
        Console.WriteLine("Status Code: " + response.StatusCode);
        if (!response.IsSuccessStatusCode)
            return Result.Failure<T>(Error.Failure(response.StatusCode.ToString(), await response.Content.ReadAsStringAsync()));

        var body = await response.Content.ReadFromJsonAsync<JsonNode>();
        Console.WriteLine("response: " + body);
        if (body!["acknowledge"]?.GetValue<string>() == "success")
        {
            var success = body["response"].Deserialize<T>(Helper.SnakeCase);
            return Result.Success<T>(success!);
        }
        else
        {
            var error = body["response"].Deserialize<ErrorResponse>();
            var errorMessages = error?.Errors
                .Select(message => Error.Failure(message, message))
                .ToArray() ?? [];
            return Result<T>.ApiFailure(new ApiError(errorMessages));
        }
    }

    public async Task<Result<BulkResponse>> BulkSendAsync(string[] recipients, string message, string campaign = "", string createCallback = "", string statusCallback = "")
    {
        var bulk = new BulkRequest(recipients, message, campaign, createCallback, statusCallback);
        var response = await client.PostAsJsonAsync("bulk_send", bulk);
        return await HandleApiResponse<BulkResponse>(response);
    }

    public async Task<Result<BulkResponse>> BulkSendAsync(Recipient[] recipients, string campaign = "", string createCallback = "", string statusCallback = "")
    {
        var bulk = new BulkSeparateRequest(recipients, campaign, createCallback, statusCallback);
        var response = await client.PostAsJsonAsync("bulk_send", bulk);
        return await HandleApiResponse<BulkResponse>(response);
    }

    public async Task<Result<MessageResponse>> SendAsync(string recipient, string message, int template = 0, string callback = "")
    {
        var sms = new MessageRequest(recipient, message, template, callback);
        var response = await client.PostAsJsonAsync("send", sms, Helper.SnakeCase);
        return await HandleApiResponse<MessageResponse>(response);
    }

    public async Task<Result<CodeResponse>> SendOTPAsync(CodeRequest request)
    {
        var query = request.ToQueryParams();
        var response = await client.GetAsync($"challenge?{query}");
        return await HandleApiResponse<CodeResponse>(response);
    }

    public async Task<Result<VerificationResponse>> VerifyCodeAsync(string recipient, string code)
    {
        var query = new { To = recipient, Code = code };
        var response = await client.GetAsync($"verify?{query.ToQueryParams()}");
        return await HandleApiResponse<VerificationResponse>(response);
    }

    public async Task<Result<VerificationResponse>> VerifyCodeAsync(Guid verificationId, string code)
    {
        var query = new { Vc = verificationId, Code = code };
        var response = await client.GetAsync($"verify?{query.ToQueryParams()}");
        return await HandleApiResponse<VerificationResponse>(response);
    }
}