using System;
using System.Linq;
using System.Collections.Generic;
using AfroMessage.Models;
using System.Threading.Tasks;
using AfroMessage.Results;

namespace AfroMessage;

public interface IAfroMessage
{
    Task<Result<MessageResponse>> SendAsync(string recipient, string message, int template = 0, string callback = "");
    Task<Result<BulkResponse>> BulkSendAsync(IEnumerable<string> recipients, string message, string campaign = "", string createCallback = "", string statusCallback = "");
    Task<Result<BulkResponse>> BulkSendAsync(IEnumerable<Recipient> recipients, string campaign = "", string createCallback = "", string statusCallback = "");
    Task<Result<CodeResponse>> SendOTPAsync(CodeRequest request);
    Task<Result<VerificationResponse>> VerifyCode(string recipient, string code);
    Task<Result<VerificationResponse>> VerifyCode(Guid verificationId, string code);
}
