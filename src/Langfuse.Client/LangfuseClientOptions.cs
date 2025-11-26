using Langfuse.Core;

namespace Langfuse.Client;

/// <summary>
/// Configuration options for the Langfuse client.
/// </summary>
public class LangfuseClientOptions : LangfuseOptions
{
    /// <summary>
    /// Gets or sets the cache TTL for prompts.
    /// Default: 60 seconds (matching official SDK behavior).
    /// </summary>
    public TimeSpan PromptCacheTtl { get; set; } = TimeSpan.FromSeconds(60);

    /// <summary>
    /// Gets or sets whether to enable prompt caching.
    /// Default: true
    /// </summary>
    public bool EnablePromptCache { get; set; } = true;

    /// <summary>
    /// Gets or sets the maximum number of retries for failed API requests.
    /// Default: 3
    /// </summary>
    public int MaxRetries { get; set; } = 3;

    /// <summary>
    /// Gets or sets the delay between retry attempts.
    /// Default: 500ms
    /// </summary>
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromMilliseconds(500);
}

