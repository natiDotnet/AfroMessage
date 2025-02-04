using AfroMessage.Models;
using AfroMessage.Results;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace AfroMessage;

public class AfroMessageClient(AfroMessageConfig config, HttpClient client) : IAfroMessage
{
    private async Task<Result<T>> HandleApiResponse<T>(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
            Result.Failure(Error.Failure(response.StatusCode.ToString(), await response.Content.ReadAsStringAsync()));

        var body = await response.Content.ReadFromJsonAsync<JsonNode>();
        Console.WriteLine("response: " + body);
        if (body["acknowledge"].GetValue<string>() == "success")
        {
            var success = body["response"].Deserialize<T>(Helper.SnakeCase);
            return Result.Success(success);
        }
        else
        {
            var error = body["response"].Deserialize<ErrorResponse>();
            var errorMessages = error.Errors
                .Select(message => Error.Failure(message, message, error.RelatedObject))
                .ToArray();
            return Result<T>.ValidationFailure(new ApiError(errorMessages));
        }
    }

    public async Task<Result<BulkResponse>> BulkSendAsync(IEnumerable<string> recipients, string message, string campaign = "", string createCallback = "", string statusCallback = "")
    {
        var bulk = new BulkRequest(recipients, message, campaign, createCallback, statusCallback);
        var response = await client.PostAsJsonAsync("bulk_send", bulk);
        return await HandleApiResponse<BulkResponse>(response);
    }

    public async Task<Result<BulkResponse>> BulkSendAsync(IEnumerable<Recipient> recipients, string campaign = "", string createCallback = "", string statusCallback = "")
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

    public async Task<Result<VerificationResponse>> VerifyCode(string recipient, string code)
    {
        var query = $"to{recipient}&code{code}";
        var response = await client.GetAsync($"verify?{query}");
        return await HandleApiResponse<VerificationResponse>(response);
    }

    public async Task<Result<VerificationResponse>> VerifyCode(Guid verificationId, string code)
    {
        var query = $"vc{verificationId}&code{code}";
        var response = await client.GetAsync($"verify?{query}");
        return await HandleApiResponse<VerificationResponse>(response);
    }
}