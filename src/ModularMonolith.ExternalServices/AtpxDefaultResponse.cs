using System.Text.Json.Serialization;

namespace ModularMonolith.ExternalServices;

public class AtpxDefaultResponse<T>
{
    [JsonPropertyName("hasErrors")]
    public bool HasErrors { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("objectResult")]
    public T? ObjectResult { get; set; }
}