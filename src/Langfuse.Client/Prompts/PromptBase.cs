using System.Text.Json;
using System.Text.RegularExpressions;

namespace Langfuse.Client.Prompts;

/// <summary>
/// Base class for Langfuse prompts.
/// </summary>
public abstract partial class PromptBase
{
    /// <summary>
    /// Gets the unique ID of the prompt.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the name of the prompt.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the version number of the prompt.
    /// </summary>
    public int Version { get; }

    /// <summary>
    /// Gets the type of prompt ("text" or "chat").
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Gets the labels associated with this prompt version.
    /// </summary>
    public IReadOnlyList<string> Labels { get; }

    /// <summary>
    /// Gets the tags associated with this prompt.
    /// </summary>
    public IReadOnlyList<string> Tags { get; }

    /// <summary>
    /// Gets the configuration object for this prompt (e.g., model parameters).
    /// </summary>
    public IReadOnlyDictionary<string, object?>? Config { get; }

    /// <summary>
    /// Gets the timestamp when this prompt was created.
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Gets the timestamp when this prompt was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; }

    /// <summary>
    /// Indicates whether this prompt is a fallback (used when API fetch fails).
    /// </summary>
    public bool IsFallback { get; protected set; }

    /// <summary>
    /// Regex pattern for matching template variables like {{variable}}.
    /// </summary>
    [GeneratedRegex(@"\{\{(\w+)\}\}", RegexOptions.Compiled)]
    private static partial Regex VariablePatternRegex();

    /// <summary>
    /// Creates a new prompt instance.
    /// </summary>
    protected PromptBase(
        string id,
        string name,
        int version,
        string type,
        IReadOnlyList<string> labels,
        IReadOnlyList<string> tags,
        IReadOnlyDictionary<string, object?>? config,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        Name = name;
        Version = version;
        Type = type;
        Labels = labels;
        Tags = tags;
        Config = config;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    /// <summary>
    /// Gets the compiled prompt template with variables substituted.
    /// </summary>
    protected static string CompileTemplate(string template, IDictionary<string, string>? variables)
    {
        if (variables == null || variables.Count == 0)
            return template;

        return VariablePatternRegex().Replace(template, match =>
        {
            var variableName = match.Groups[1].Value;
            return variables.TryGetValue(variableName, out var value)
                ? value
                : match.Value; // Keep original if not found
        });
    }

    /// <summary>
    /// Gets a configuration value as a specific type.
    /// </summary>
    /// <typeparam name="T">The type to convert to.</typeparam>
    /// <param name="key">The configuration key.</param>
    /// <param name="defaultValue">Default value if key not found.</param>
    /// <returns>The configuration value or default.</returns>
    public T? GetConfigValue<T>(string key, T? defaultValue = default)
    {
        if (Config == null || !Config.TryGetValue(key, out var value) || value == null)
            return defaultValue;

        if (value is T typedValue)
            return typedValue;

        if (value is JsonElement element)
        {
            try
            {
                return element.Deserialize<T>();
            }
            catch
            {
                return defaultValue;
            }
        }

        try
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Converts the prompt to a JSON representation for linking with traces.
    /// </summary>
    public Dictionary<string, object?> ToJson()
    {
        return new Dictionary<string, object?>
        {
            ["id"] = Id,
            ["name"] = Name,
            ["version"] = Version,
            ["type"] = Type,
            ["labels"] = Labels,
            ["tags"] = Tags,
            ["config"] = Config
        };
    }
}

