using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace AfroMessage.Responses;

public class CodeResponse
{
    public string? Status { get; set; }
    public Guid? MessageId { get; set; }
    public string? Message { get; set; }
    public string? Recipient { get; set; }
    public string? Code { get; set; }
    [JsonPropertyName("verificationId")]
    public Guid? VerificationId { get; set; }
}
