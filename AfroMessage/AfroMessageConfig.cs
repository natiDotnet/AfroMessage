namespace AfroMessage;

public class AfroMessageConfig
{
    public const string Key = "AfroMessage";
    public const string Url = "https://api.afromessage.com/api/";
    public string Token { get; set; } = string.Empty;
    public string Sender { get; set; } = string.Empty;
    public string Identifier { get; set; } = string.Empty;
}
