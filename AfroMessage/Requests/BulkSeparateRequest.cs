using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AfroMessage.Requests;

public record BulkSeparateRequest(
    Recipient[] Recipients,
    string Campaign,
    string CreateCallback,
    string StatusCallback)
{
    [JsonPropertyName("to")]
    public Recipient[] Recipients { get; set; } = Recipients;
}
