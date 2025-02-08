using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AfroMessage.Requests;

public record MessageRequest(string Recipient, string Message, int Template, string Callback)
{
    [JsonPropertyName("to")]
    public string Recipient { get; set; } = Recipient;
}
