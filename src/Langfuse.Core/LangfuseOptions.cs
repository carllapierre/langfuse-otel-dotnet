namespace Langfuse.Core;

/// <summary>
/// Configuration options for Langfuse SDK.
/// </summary>
public class LangfuseOptions
{
    /// <summary>
    /// Gets or sets the base URL of the Langfuse instance.
    /// Default: https://cloud.langfuse.com
    /// Can be set via environment variable: LANGFUSE_BASE_URL
    /// </summary>
    public string? BaseUrl { get; set; }

    /// <summary>
    /// Gets or sets the Langfuse public key for authentication.
    /// Can be set via environment variable: LANGFUSE_PUBLIC_KEY
    /// </summary>
    public string? PublicKey { get; set; }

    /// <summary>
    /// Gets or sets the Langfuse secret key for authentication.
    /// Can be set via environment variable: LANGFUSE_SECRET_KEY
    /// </summary>
    public string? SecretKey { get; set; }

    /// <summary>
    /// Gets or sets the default timeout for HTTP requests.
    /// Default: 30 seconds
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Creates a validated copy of these options with defaults applied.
    /// </summary>
    /// <returns>A new LangfuseOptions instance with defaults applied.</returns>
    public LangfuseOptions WithDefaults()
    {
        return new LangfuseOptions
        {
            BaseUrl = string.IsNullOrWhiteSpace(BaseUrl) ? LangfuseConstants.DefaultBaseUrl : BaseUrl.TrimEnd('/'),
            PublicKey = PublicKey,
            SecretKey = SecretKey,
            Timeout = Timeout
        };
    }

    /// <summary>
    /// Validates that required options are set.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if required options are missing.</exception>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(PublicKey))
        {
            throw new InvalidOperationException(
                $"Langfuse PublicKey is required. Set it via code, environment variable ({LangfuseConstants.EnvPublicKey}), or configuration.");
        }

        if (string.IsNullOrWhiteSpace(SecretKey))
        {
            throw new InvalidOperationException(
                $"Langfuse SecretKey is required. Set it via code, environment variable ({LangfuseConstants.EnvSecretKey}), or configuration.");
        }
    }
}

