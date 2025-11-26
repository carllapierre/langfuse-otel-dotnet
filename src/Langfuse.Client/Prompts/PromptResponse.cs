using System.Text.Json;
using System.Text.Json.Serialization;

namespace Langfuse.Client.Prompts;

/// <summary>
/// Internal response model from Langfuse Prompts API.
/// </summary>
internal class PromptApiResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("prompt")]
    public JsonElement Prompt { get; set; }

    [JsonPropertyName("config")]
    public JsonElement? Config { get; set; }

    [JsonPropertyName("labels")]
    public List<string> Labels { get; set; } = new();

    [JsonPropertyName("tags")]
    public List<string> Tags { get; set; } = new();

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; set; }
}

