using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AfroMessage.Requests;
using AfroMessage.Responses;
using AfroMessage.Results;

namespace AfroMessage;

public interface IAfroMessageClient
{
    Task<Result<MessageResponse>> SendAsync(string recipient, string message, int template = 0, string callback = "");
    Task<Result<BulkResponse>> BulkSendAsync(string[] recipients, string message, string campaign = "", string createCallback = "", string statusCallback = "");
    Task<Result<BulkResponse>> BulkSendAsync(Recipient[] recipients, string campaign = "", string createCallback = "", string statusCallback = "");
    Task<Result<CodeResponse>> SendOTPAsync(CodeRequest request);
    Task<Result<VerificationResponse>> VerifyCodeAsync(string recipient, string code);
    Task<Result<VerificationResponse>> VerifyCodeAsync(Guid verificationId, string code);
}