using System.Text.Json;
using System.Text.Json.Nodes;
using System.Web;

namespace AfroMessage.Results;

public static class Helper
{
    public static JsonSerializerOptions SnakeCase =
    new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        WriteIndented = true
    };
    public static string ToQueryParams(this object obj)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));

        var json = JsonSerializer.Serialize(obj, SnakeCase);
        var jsonObject = JsonNode.Parse(json)?.AsObject();

        // Build query parameters
        var query = HttpUtility.ParseQueryString(string.Empty);
        foreach (var property in jsonObject)
        {
            var value = property.Value?.ToString();
            Console.WriteLine("Vlaue: " + value);
            if (!string.IsNullOrEmpty(value))
            {
                query[property.Key] = HttpUtility.UrlEncode(value);
            }
        }

        return query.ToString();
    }

    public static string[] GetErrors(this Result result)
    {
        if (result.Error is not ApiError errors)
            return null;
        return errors.Errors
            .Select(e => e.Description)
            .ToArray();
    }
}
