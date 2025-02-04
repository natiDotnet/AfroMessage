using System.Text.Json.Serialization;

namespace AfroMessage.Models;

public class MessageResponse
{
    public string Status { get; set; }
    public string MessageId { get; set; }
    public string Message { get; set; }
    public string To { get; set; }
}

public class BulkResponse
{
    public string Message { get; set; }
    public string CampaignId { get; set; }
    public string[]? Errors { get; set; }
}

public class CodeResponse
{
    public string Status { get; set; }
    public string MessageId { get; set; }
    public string Message { get; set; }
    public string Recipient { get; set; }
    public string Code { get; set; }
    [JsonPropertyName("verificationId")]
    public string VerificationId { get; set; }
}
public class VerificationResponse
{
    public string Phone { get; set; }
    public string Code { get; set; }
    [JsonPropertyName("verificationId")]
    public string VerificationId { get; set; }
    [JsonPropertyName("sentAt")]
    public string SentAt { get; set; }
}
public class ErrorResponse
{
    [JsonPropertyName("errors")]
    public string[] Errors { get; set; }
    [JsonPropertyName("relatedObject")]
    public string? RelatedObject { get; set; }
}
