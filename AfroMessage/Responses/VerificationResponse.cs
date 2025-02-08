using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace AfroMessage.Responses;

public class VerificationResponse
{
    public string? Phone { get; set; }

    public string? Code { get; set; }

    [JsonPropertyName("verificationId")]
    public Guid VerificationId { get; set; }

    [JsonPropertyName("sentAt")]
    public string? SentAt { get; set; }
}
