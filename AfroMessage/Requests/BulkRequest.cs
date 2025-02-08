using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using AfroMessage.Requests;

namespace AfroMessage.Requests;

public class BulkRequest(
    string[] Recipients,
    string Message,
    string Campaign,
    string CreateCallback,
    string StatusCallback)
{
    [JsonPropertyName("to")]
    public string[] Recipients { get; set; } = Recipients;
}