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
        return result.Error switch
        {
            var err when err == Error.None => [],  // Return empty array for "no error"
            ApiError apiError => apiError.Errors?
                .Select(e => e.Description)
                .ToArray() ?? [],
            _ => [result.Error.Description]  // For all other errors
        };
    }
}
