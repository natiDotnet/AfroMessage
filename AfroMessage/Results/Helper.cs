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
        // Build query parameters
        var query = HttpUtility.ParseQueryString(string.Empty);
        foreach (var property in obj.GetType().GetProperties())
        {
            var value = property.GetValue(obj)?.ToString();
            if (!string.IsNullOrEmpty(value))
            {
                query[property.Name] = HttpUtility.UrlEncode(value);
            }
        }

        return query.ToString()!;
    }

    public static string[] GetErrors(this Result result)
    {
        if (result.Error is not ApiError errors)
            return [result.Error.Description];
        return errors.Errors
            .Select(e => e.Description)
            .ToArray();
    }
}
