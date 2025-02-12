# Afro Message .NET API Client

A C# library for interacting with the [AfroMessage API](https://afromessage.com/developers). This package provides an easy-to-use interface to send messages, manage bulk messaging campaigns, and handle verification codes.

## Installation

You can install this package via NuGet:

```bash
dotnet add package AfroMessage
```

Or via the NuGet Package Manager Console:

```powershell
Install-Package AfroMessage
```

## Getting Started

Before using the library, ensure you have an AfroMessage account and API key. You can obtain these from the [AfroMessage Developer Portal](https://afromessage.com/developers).

## Usage

## Using AfroMessageClient

The `AfroMessageClient` can be used in two primary ways: by directly instantiating it using the `new` keyword or by leveraging Dependency Injection (DI) through the service container.

### 1. Using `new` Keyword

You can create an instance of `AfroMessageClient` directly by providing the required configuration:

#### Example Configuration

```csharp
var config = new AfroMessageConfig
{
    Token = "your-token",
    Sender = "1234",
    Identifier = "e80ad9d8-adf3-463f-80f4-7c8373773"
};
var afro = new AfroMessageClient(config);
// or 
var afro = new AfroMessageClient(token: config.Token, identifier: config.Identifier, sender: config.Sender);
```
### 2. Using Dependency Injection (DI)

To use `AfroMessageClient` with Dependency Injection, configure it in the service collection during application startup:
```csharp
var config = new AfroMessageConfig
{
    Token = "your-token",
    Sender = "1234",
    Identifier = "e80ad9d8-adf3-463f-80f4-7c8373773"
};
builder.Services.AddAfroMessage(config);
```
### Sending a Single Message Using DI

Once registered, you can inject IAfroMessageClient into your handlers or controllers:
```csharp
app.MapGet("/SendMessage/{phone}/DI", async (string phone, IAfroMessageClient afro) => 
{
    var result = await afro.SendMessageAsync(phone, "Hello 👋");
    return result.IsFailure ?
        Results.BadRequest(result.Error) :
        Results.Ok(result.Value);
})
.WithOpenApi();
```

### Send a Single Message

Send a message to a single recipient:

```csharp
var result = await client.SendMessageAsync("1234567890", "Hello, this is a test message!");

if (result.IsSuccess)
{
    Console.WriteLine($"Message sent successfully! Message ID: {result.Value.MessageId}");
}
else
{
    Console.WriteLine($"Error: {result.Error.Message}");
}
```

### Send Bulk Messages

Send messages to multiple recipients:

#### Using an Array of Recipient Phone Numbers

```csharp
var recipients = new[] { "1234567890", "0987654321" };
var result = await client.SendBulkMessageAsync(recipients, "This is a bulk message!");

if (result.IsSuccess)
{
    Console.WriteLine($"Bulk message sent successfully! Campaign ID: {result.Value.CampaignId}");
}
else
{
    Console.WriteLine($"Error: {result.Error.Message}");
}
```

#### Using Custom Recipient Objects

```csharp
var recipients = new[]
{
    new Recipient { To = "1234567890", Message = "message to recipient one" },
    new Recipient { To = "0987654321", Message = "message to recipient two" }
};

var result = await client.SendBulkMessageAsync(recipients, campaign: "My Bulk Campaign");

if (result.IsSuccess)
{
    Console.WriteLine($"Bulk message sent successfully! Campaign ID: {result.Value.CampaignId}");
}
else
{
    Console.WriteLine($"Error: {result.Error.Message}");
}
```

### Send a Verification Code

Send a verification code to a recipient:

```csharp
var request = new CodeRequest
{
    Recipient = "1234567890",
    CodeLength = 4,
    CodeType = CodeType.Numeric,
    ExpirationValue  = TimeSpan.FromMinutes(5)
};

var result = await client.SendCodeAsync(request);

if (result.IsSuccess)
{
    Console.WriteLine($"Verification code sent successfully! Verification ID: {result.Value.VerificationId}");
}
else
{
    Console.WriteLine($"Error: {result.Error.Message}");
}
```

### Verify a Code

Verify a code provided by the recipient:

#### By Recipient Phone Number

```csharp
var result = await client.VerifyCodeAsync("1234567890", "123456");

if (result.IsSuccess)
{
    Console.WriteLine("Code verified successfully!");
}
else
{
    Console.WriteLine($"Error: {result.Error.Message}");
}
```

#### By Verification ID

```csharp
var verificationId = Guid.Parse("your_verification_id_here");
var result = await client.VerifyCodeAsync(verificationId, "123456");

if (result.IsSuccess)
{
    Console.WriteLine("Code verified successfully!");
}
else
{
    Console.WriteLine($"Error: {result.Error.Message}");
}
```

## Response Handling

The library uses a generic `Result<T>` type to encapsulate API responses. This allows you to easily check for success or errors:

- `IsSuccess`: Indicates whether the operation was successful.
- `Value`: Contains the response data if the operation succeeded.
- `Error`: Contains error details if the operation failed.

Example:

```csharp
var result = await client.SendMessageAsync("1234567890", "Test message");

if (result.IsSuccess)
{
    Console.WriteLine($"Message ID: {result.Value.MessageId}");
}
else
{
    Console.WriteLine($"Error: {result.Error.Code} - {result.Error.Message}");
}
```

## Contributing

Contributions are welcome! If you find any issues or have suggestions for improvement, please open an issue or submit a pull request.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

