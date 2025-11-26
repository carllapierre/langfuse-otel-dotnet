using System.Text.Json.Serialization;

namespace Langfuse.Client.Prompts;

/// <summary>
/// Represents a message in a chat prompt.
/// </summary>
public class PromptMessage
{
    /// <summary>
    /// Gets or sets the role of the message (e.g., "system", "user", "assistant").
    /// </summary>
    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the content of the message.
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Creates a new prompt message.
    /// </summary>
    public PromptMessage() { }

    /// <summary>
    /// Creates a new prompt message with the specified role and content.
    /// </summary>
    /// <param name="role">The role (system, user, assistant).</param>
    /// <param name="content">The message content.</param>
    public PromptMessage(string role, string content)
    {
        Role = role;
        Content = content;
    }

    /// <summary>
    /// Creates a system message.
    /// </summary>
    public static PromptMessage System(string content) => new("system", content);

    /// <summary>
    /// Creates a user message.
    /// </summary>
    public static PromptMessage User(string content) => new("user", content);

    /// <summary>
    /// Creates an assistant message.
    /// </summary>
    public static PromptMessage Assistant(string content) => new("assistant", content);
}

