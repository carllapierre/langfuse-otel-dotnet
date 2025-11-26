using Langfuse.Core;

namespace Langfuse.IntegrationTests;

/// <summary>
/// Configuration helper for integration tests.
/// </summary>
public static class TestConfiguration
{
    /// <summary>
    /// Gets test configuration from environment variables.
    /// For cloud tests, expects LANGFUSE_PUBLIC_KEY, LANGFUSE_SECRET_KEY, and optionally LANGFUSE_BASE_URL.
    /// For local tests, uses docker-compose values.
    /// </summary>
    public static LangfuseOptions GetCloudOptions()
    {
        var publicKey = Environment.GetEnvironmentVariable(LangfuseConstants.EnvPublicKey);
        var secretKey = Environment.GetEnvironmentVariable(LangfuseConstants.EnvSecretKey);
        var baseUrl = Environment.GetEnvironmentVariable(LangfuseConstants.EnvBaseUrl)
                      ?? LangfuseConstants.DefaultBaseUrl;

        if (string.IsNullOrEmpty(publicKey) || string.IsNullOrEmpty(secretKey))
        {
            throw new SkipException("Cloud integration tests require LANGFUSE_PUBLIC_KEY and LANGFUSE_SECRET_KEY environment variables.");
        }

        return new LangfuseOptions
        {
            BaseUrl = baseUrl,
            PublicKey = publicKey,
            SecretKey = secretKey
        };
    }

    /// <summary>
    /// Gets configuration for local Docker-based Langfuse instance.
    /// </summary>
    public static LangfuseOptions GetLocalOptions(string baseUrl = "http://localhost:3003")
    {
        // Default keys for local development - these match the docker-compose defaults
        return new LangfuseOptions
        {
            BaseUrl = baseUrl,
            PublicKey = Environment.GetEnvironmentVariable("LANGFUSE_LOCAL_PUBLIC_KEY") ?? "pk-lf-local",
            SecretKey = Environment.GetEnvironmentVariable("LANGFUSE_LOCAL_SECRET_KEY") ?? "sk-lf-local"
        };
    }

    /// <summary>
    /// Checks if cloud integration tests should run.
    /// </summary>
    public static bool IsCloudConfigured()
    {
        var publicKey = Environment.GetEnvironmentVariable(LangfuseConstants.EnvPublicKey);
        var secretKey = Environment.GetEnvironmentVariable(LangfuseConstants.EnvSecretKey);
        return !string.IsNullOrEmpty(publicKey) && !string.IsNullOrEmpty(secretKey);
    }

    /// <summary>
    /// Checks if local integration tests should run.
    /// </summary>
    public static bool IsLocalConfigured()
    {
        var publicKey = Environment.GetEnvironmentVariable("LANGFUSE_LOCAL_PUBLIC_KEY");
        var secretKey = Environment.GetEnvironmentVariable("LANGFUSE_LOCAL_SECRET_KEY");
        return !string.IsNullOrEmpty(publicKey) && !string.IsNullOrEmpty(secretKey);
    }
}

/// <summary>
/// Exception to skip tests when configuration is missing.
/// </summary>
public class SkipException : Exception
{
    public SkipException(string message) : base(message) { }
}

