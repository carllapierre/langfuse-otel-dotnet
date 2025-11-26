namespace Langfuse.Client.Prompts;

/// <summary>
/// Represents a text prompt from Langfuse.
/// </summary>
public class TextPrompt : PromptBase
{
    /// <summary>
    /// Gets the raw prompt template string including {{variables}}.
    /// </summary>
    public string Prompt { get; }

    /// <summary>
    /// Creates a new text prompt instance.
    /// </summary>
    internal TextPrompt(
        string id,
        string name,
        int version,
        string prompt,
        IReadOnlyList<string> labels,
        IReadOnlyList<string> tags,
        IReadOnlyDictionary<string, object?>? config,
        DateTime createdAt,
        DateTime updatedAt,
        bool isFallback = false)
        : base(id, name, version, "text", labels, tags, config, createdAt, updatedAt)
    {
        Prompt = prompt;
        IsFallback = isFallback;
    }

    /// <summary>
    /// Compiles the prompt by substituting variables with provided values.
    /// </summary>
    /// <param name="variables">Dictionary of variable names and their values.</param>
    /// <returns>The compiled prompt string.</returns>
    /// <example>
    /// <code>
    /// var prompt = await client.GetPromptAsync("movie-critic");
    /// var compiled = prompt.Compile(new Dictionary&lt;string, string&gt;
    /// {
    ///     ["criticlevel"] = "expert",
    ///     ["movie"] = "Dune 2"
    /// });
    /// // "As an expert movie critic, do you like Dune 2?"
    /// </code>
    /// </example>
    public string Compile(IDictionary<string, string>? variables = null)
    {
        return CompileTemplate(Prompt, variables);
    }

    /// <summary>
    /// Compiles the prompt by substituting variables using named arguments.
    /// </summary>
    /// <param name="variables">Variable values as key-value pairs.</param>
    /// <returns>The compiled prompt string.</returns>
    public string Compile(params (string Key, string Value)[] variables)
    {
        var dict = variables.ToDictionary(v => v.Key, v => v.Value);
        return Compile(dict);
    }

    /// <summary>
    /// Creates a fallback text prompt for use when API fetch fails.
    /// </summary>
    /// <param name="name">The prompt name.</param>
    /// <param name="prompt">The fallback prompt template.</param>
    /// <returns>A fallback TextPrompt instance.</returns>
    public static TextPrompt CreateFallback(string name, string prompt)
    {
        return new TextPrompt(
            id: $"fallback-{Guid.NewGuid():N}",
            name: name,
            version: 0,
            prompt: prompt,
            labels: Array.Empty<string>(),
            tags: Array.Empty<string>(),
            config: null,
            createdAt: DateTime.UtcNow,
            updatedAt: DateTime.UtcNow,
            isFallback: true);
    }
}

