using System;
using System.Linq;
using System.Collections.Generic;

namespace AfroMessage;

public class AfroMessageConfig
{
    public const string Key = "AfroMessage";
    public const string Url = "https://api.afromessage.com/api/";
    public string Token { get; set; }
    public string Sender { get; set; }
    public string? Identifier { get; set; }
}
