using AfroMessage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var config = new AfroMessageConfig
{
    Token = "eyJhbGciOiJIUzI1NiJ9.eyJpZGVudGlmaWVyIjoiMjJTQkdyN2RLVWsyMERrbWU5bkw0ZWNXQXRaQXlxVFgiLCJleHAiOjE4OTU5OTQ2ODEsImlhdCI6MTczODIyODI4MSwianRpIjoiMzI5YzY2OGEtYWY4ZS00YTA2LThlZWUtZTlkOGIyYTQyY2I3In0.qBVzEMsYpm-As2m8dcDpCcLm9HlvykjRdVRsaI-LFrs",
    Sender = "9786",
    Identifier = "e80ad9d8-adf3-463f-80f4-7c4b39f7f164"
};
builder.Services.AddAfroMessage(config);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/SayHello/{phone}/Config", async (string phone) => 
{
    var afro = new AfroMessageClient(config);
    var result = await afro.SendAsync(phone, "Hello 👋");
    return result.IsFailure ?
        Results.BadRequest(result.Error) :
        Results.Ok(result.Value);
})
.WithOpenApi();
app.MapGet("/SayHello/{phone}/Token", async (string phone) => 
{
    var afro = new AfroMessageClient(token: config.Token, identifier: config.Identifier, sender: config.Sender);
    var result = await afro.SendAsync(phone, "Hello 👋");
    return result.IsFailure ?
        Results.BadRequest(result.Error) :
        Results.Ok(result.Value);
})
.WithOpenApi();

app.MapGet("/SayHello/{phone}/DI", async (string phone, IAfroMessageClient afro) => 
{
    var result = await afro.SendAsync(phone, "Hello 👋");
    return result.IsFailure ?
        Results.BadRequest(result.Error) :
        Results.Ok(result.Value);
})
.WithOpenApi();
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
