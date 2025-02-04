namespace AfroMessage.Results;

public record Error
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
    public static readonly Error NullValue = new(
        "General.Null",
        "Null value was provided",
        ErrorType.Failure);

    public Error(string code, string description, ErrorType type, string? relatedObject = null)
    {
        Code = code;
        Description = description;
        Type = type;
        RelatedObject = relatedObject;
    }

    public string Code { get; }

    public string Description { get; }
    public string? RelatedObject { get; set; }

    public ErrorType Type { get; }

    public static Error Failure(string code, string description, string? relatedObject = null) =>
        new(code, description, ErrorType.Failure, relatedObject);
}
