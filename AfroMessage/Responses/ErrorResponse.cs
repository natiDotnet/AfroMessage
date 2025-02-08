using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace AfroMessage.Responses;

public class ErrorResponse
{
    [JsonPropertyName("errors")]
    public string[] Errors { get; set; } = [];
}
