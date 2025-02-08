using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace AfroMessage.Requests;

public class CodeRequest
{
    [JsonPropertyName("to")]
    public required string Recipient { get; set; }

    [JsonPropertyName("pr")]
    public string? MessagePrefix { get; set; }

    [JsonPropertyName("ps")]
    public string? MessagePostfix { get; set; }

    [JsonPropertyName("sb")]
    public int? SpacesBefore { get; set; }

    [JsonPropertyName("sa")]
    public int? SpacesAfter { get; set; }

    [JsonInclude]
    [JsonPropertyName("ttl")]
    private int Expiration
    {
        get => (int)ExpirationValue.TotalSeconds;
        set => ExpirationValue = TimeSpan.FromSeconds(value);
    }
    [JsonIgnore]
    public TimeSpan ExpirationValue { get; set; }

    [JsonPropertyName("len")]
    public int CodeLength { get; set; }

    [JsonPropertyName("t")]
    public required CodeType CodeType { get; set; }

    [JsonPropertyName("callback")]
    public string? Callback { get; set; }
}

public enum CodeType
{
    Numeric,
    Alphabet,
    AlphaNumeric

}
