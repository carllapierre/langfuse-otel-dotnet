using System.Text;

namespace Langfuse.Core;

/// <summary>
/// Helper for Langfuse authentication.
/// </summary>
public static class AuthenticationHelper
{
    /// <summary>
    /// Builds the Basic Authentication header value from public and secret keys.
    /// </summary>
    /// <param name="publicKey">Langfuse public key.</param>
    /// <param name="secretKey">Langfuse secret key.</param>
    /// <returns>Basic auth header value (e.g., "Basic base64encoded").</returns>
    public static string BuildBasicAuthHeader(string publicKey, string secretKey)
    {
        var credentials = $"{publicKey}:{secretKey}";
        var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
        return $"Basic {encoded}";
    }

    /// <summary>
    /// Builds the Authorization header value from options.
    /// </summary>
    /// <param name="options">Langfuse options containing keys.</param>
    /// <returns>Basic auth header value.</returns>
    /// <exception cref="InvalidOperationException">Thrown if keys are not configured.</exception>
    public static string BuildBasicAuthHeader(LangfuseOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.PublicKey) || string.IsNullOrWhiteSpace(options.SecretKey))
        {
            throw new InvalidOperationException("PublicKey and SecretKey must be configured.");
        }

        return BuildBasicAuthHeader(options.PublicKey, options.SecretKey);
    }
}

