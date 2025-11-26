namespace Langfuse.Client.Prompts;

/// <summary>
/// Represents a chat prompt from Langfuse with multiple messages.
/// </summary>
public class ChatPrompt : PromptBase
{
    /// <summary>
    /// Gets the raw chat messages including {{variables}}.
    /// </summary>
    public IReadOnlyList<PromptMessage> Prompt { get; }

    /// <summary>
    /// Creates a new chat prompt instance.
    /// </summary>
    internal ChatPrompt(
        string id,
        string name,
        int version,
        IReadOnlyList<PromptMessage> messages,
        IReadOnlyList<string> labels,
        IReadOnlyList<string> tags,
        IReadOnlyDictionary<string, object?>? config,
        DateTime createdAt,
        DateTime updatedAt,
        bool isFallback = false)
        : base(id, name, version, "chat", labels, tags, config, createdAt, updatedAt)
    {
        Prompt = messages;
        IsFallback = isFallback;
    }

    /// <summary>
    /// Compiles the chat messages by substituting variables with provided values.
    /// </summary>
    /// <param name="variables">Dictionary of variable names and their values.</param>
    /// <returns>A list of compiled messages.</returns>
    /// <example>
    /// <code>
    /// var prompt = await client.GetChatPromptAsync("movie-critic-chat");
    /// var compiled = prompt.Compile(new Dictionary&lt;string, string&gt;
    /// {
    ///     ["criticlevel"] = "expert",
    ///     ["movie"] = "Dune 2"
    /// });
    /// // [{ role: "system", content: "You are an expert movie critic" }, ...]
    /// </code>
    /// </example>
    public IReadOnlyList<PromptMessage> Compile(IDictionary<string, string>? variables = null)
    {
        if (variables == null || variables.Count == 0)
        {
            return Prompt;
        }

        return Prompt.Select(m => new PromptMessage
        {
            Role = m.Role,
            Content = CompileTemplate(m.Content, variables)
        }).ToList();
    }

    /// <summary>
    /// Compiles the chat messages by substituting variables using named arguments.
    /// </summary>
    /// <param name="variables">Variable values as key-value pairs.</param>
    /// <returns>A list of compiled messages.</returns>
    public IReadOnlyList<PromptMessage> Compile(params (string Key, string Value)[] variables)
    {
        var dict = variables.ToDictionary(v => v.Key, v => v.Value);
        return Compile(dict);
    }

    /// <summary>
    /// Creates a fallback chat prompt for use when API fetch fails.
    /// </summary>
    /// <param name="name">The prompt name.</param>
    /// <param name="messages">The fallback messages.</param>
    /// <returns>A fallback ChatPrompt instance.</returns>
    public static ChatPrompt CreateFallback(string name, IReadOnlyList<PromptMessage> messages)
    {
        return new ChatPrompt(
            id: $"fallback-{Guid.NewGuid():N}",
            name: name,
            version: 0,
            messages: messages,
            labels: Array.Empty<string>(),
            tags: Array.Empty<string>(),
            config: null,
            createdAt: DateTime.UtcNow,
            updatedAt: DateTime.UtcNow,
            isFallback: true);
    }
}

