using System.Text.Json;

namespace Langfuse.Client.Prompts;

/// <summary>
/// Factory for creating prompt instances from API responses.
/// </summary>
internal static class PromptFactory
{
    /// <summary>
    /// Creates a text prompt from an API response.
    /// </summary>
    public static TextPrompt CreateTextPrompt(PromptApiResponse response)
    {
        var promptText = response.Prompt.ValueKind == JsonValueKind.String
            ? response.Prompt.GetString() ?? string.Empty
            : response.Prompt.GetRawText();

        return new TextPrompt(
            id: response.Id,
            name: response.Name,
            version: response.Version,
            prompt: promptText,
            labels: response.Labels,
            tags: response.Tags,
            config: ParseConfig(response.Config),
            createdAt: response.CreatedAt,
            updatedAt: response.UpdatedAt);
    }

    /// <summary>
    /// Creates a chat prompt from an API response.
    /// </summary>
    public static ChatPrompt CreateChatPrompt(PromptApiResponse response)
    {
        var messages = ParseChatMessages(response.Prompt);

        return new ChatPrompt(
            id: response.Id,
            name: response.Name,
            version: response.Version,
            messages: messages,
            labels: response.Labels,
            tags: response.Tags,
            config: ParseConfig(response.Config),
            createdAt: response.CreatedAt,
            updatedAt: response.UpdatedAt);
    }

    private static List<PromptMessage> ParseChatMessages(JsonElement prompt)
    {
        var messages = new List<PromptMessage>();

        if (prompt.ValueKind != JsonValueKind.Array)
            return messages;

        foreach (var element in prompt.EnumerateArray())
        {
            var role = element.TryGetProperty("role", out var roleElement)
                ? roleElement.GetString() ?? "user"
                : "user";

            var content = element.TryGetProperty("content", out var contentElement)
                ? contentElement.GetString() ?? string.Empty
                : string.Empty;

            messages.Add(new PromptMessage(role, content));
        }

        return messages;
    }

    private static Dictionary<string, object?>? ParseConfig(JsonElement? config)
    {
        if (config == null || config.Value.ValueKind == JsonValueKind.Null)
            return null;

        var result = new Dictionary<string, object?>();

        if (config.Value.ValueKind != JsonValueKind.Object)
            return result;

        foreach (var property in config.Value.EnumerateObject())
        {
            result[property.Name] = ParseJsonValue(property.Value);
        }

        return result;
    }

    private static object? ParseJsonValue(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number => element.TryGetInt64(out var l) ? l : element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            JsonValueKind.Array => element.EnumerateArray().Select(ParseJsonValue).ToList(),
            JsonValueKind.Object => element.EnumerateObject()
                .ToDictionary(p => p.Name, p => ParseJsonValue(p.Value)),
            _ => element.GetRawText()
        };
    }
}

