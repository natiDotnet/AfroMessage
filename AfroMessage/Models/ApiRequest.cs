using System;
using System.Linq;
using System.Collections.Generic;

namespace AfroMessage.Models;

using System.Text.Json.Serialization;

public class Recipient
{
    public string To { get; set; }
    public string Message { get; set; }
}

public class MessageRequest
{
    public MessageRequest(string recipient, string message, int template, string callback)
    {
        To = recipient;
        Message = message;
        Template = template;
        Callback = callback;
    }
    // Identifier ID for the sender
    public string From { get; set; }

    // Name of the sender
    public string Sender { get; set; }

    // Recipient phone number
    public string To { get; set; }

    // Message content
    public string Message { get; set; }

    // Message template
    public int Template { get; set; }

    // Callback URL
    public string Callback { get; set; }
}

public class BulkRequest
{
    public BulkRequest(IEnumerable<string> recipients, string message, string campaign, string createCallback, string statusCallback)
    {
        To = recipients;
        Message = message;
        Campaign = campaign;
        CreateCallback = createCallback;
        StatusCallback = statusCallback;
    }
    // List of phone numbers to send the message to
    public IEnumerable<string> To { get; set; }

    // The message content
    public string Message { get; set; }

    // Identifier ID for the sender
    public string From { get; set; }

    // Name of the sender
    public string Sender { get; set; }

    // Name of the campaign
    public string Campaign { get; set; }

    // Callback URL for campaign creation
    public string CreateCallback { get; set; }

    // Callback URL for message status updates
    public string StatusCallback { get; set; }
}

public class BulkSeparateRequest
{
    public BulkSeparateRequest(IEnumerable<Recipient> recipients, string campaign, string createCallback, string statusCallback)
    {
        To = recipients;
        Campaign = campaign;
        CreateCallback = createCallback;
        StatusCallback = statusCallback;
    }
    // List of recipient objects, each containing a "to" and "message" field
    public IEnumerable<Recipient> To { get; set; }

    // Identifier ID for the sender
    public string From { get; set; }

    // Name of the sender
    public string Sender { get; set; }

    // Name of the campaign
    public string Campaign { get; set; }

    // Callback URL for campaign creation
    public string CreateCallback { get; set; }

    // Callback URL for message status updates
    public string StatusCallback { get; set; }
}

public class CodeRequest
{
    //[JsonPropertyName("from")]
    //public string IdentifierId { get; set; }  

    //[JsonPropertyName("sender")]
    //public string SenderName { get; set; }    

    [JsonPropertyName("to")]
    public string Recipient { get; set; }

    [JsonPropertyName("pr")]
    public string MessagePrefix { get; set; }

    [JsonPropertyName("ps")]
    public string MessagePostfix { get; set; }

    [JsonPropertyName("sb")]
    public int SpacesBefore { get; set; }

    [JsonPropertyName("sa")]
    public int SpacesAfter { get; set; }

    [JsonPropertyName("ttl")]
    public int ExpirationValue { get; set; }

    [JsonPropertyName("len")]
    public int CodeLength { get; set; }

    [JsonPropertyName("t")]
    public string CodeType { get; set; }

    [JsonPropertyName("callback")]
    public string CallbackMessage { get; set; }
}