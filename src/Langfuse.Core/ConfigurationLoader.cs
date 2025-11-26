using Microsoft.Extensions.Configuration;

namespace Langfuse.Core;

/// <summary>
/// Helper to load Langfuse configuration from various sources.
/// Priority order: Code-based options > Environment variables > IConfiguration > Defaults
/// </summary>
public static class ConfigurationLoader
{
    /// <summary>
    /// Loads configuration from environment variables and IConfiguration, applying defaults.
    /// </summary>
    /// <param name="configuration">Optional IConfiguration instance (e.g., from appsettings.json).</param>
    /// <returns>Configured LangfuseOptions instance.</returns>
    public static LangfuseOptions Load(IConfiguration? configuration = null)
    {
        var options = new LangfuseOptions();

        // Try configuration first (appsettings.json, etc.)
        if (configuration != null)
        {
            var section = configuration.GetSection(LangfuseConstants.ConfigurationSectionName);
            if (section.Exists())
            {
                options.BaseUrl = section[nameof(LangfuseOptions.BaseUrl)];
                options.PublicKey = section[nameof(LangfuseOptions.PublicKey)];
                options.SecretKey = section[nameof(LangfuseOptions.SecretKey)];

                var timeoutStr = section[nameof(LangfuseOptions.Timeout)];
                if (!string.IsNullOrEmpty(timeoutStr) && int.TryParse(timeoutStr, out var timeoutSeconds))
                {
                    options.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
                }
            }
        }

        // Override with environment variables (higher priority)
        var envBaseUrl = Environment.GetEnvironmentVariable(LangfuseConstants.EnvBaseUrl);
        var envPublicKey = Environment.GetEnvironmentVariable(LangfuseConstants.EnvPublicKey);
        var envSecretKey = Environment.GetEnvironmentVariable(LangfuseConstants.EnvSecretKey);

        if (!string.IsNullOrEmpty(envBaseUrl))
            options.BaseUrl = envBaseUrl;
        if (!string.IsNullOrEmpty(envPublicKey))
            options.PublicKey = envPublicKey;
        if (!string.IsNullOrEmpty(envSecretKey))
            options.SecretKey = envSecretKey;

        // Apply defaults
        return options.WithDefaults();
    }

    /// <summary>
    /// Loads configuration and validates that required values are present.
    /// </summary>
    /// <param name="configuration">Optional IConfiguration instance.</param>
    /// <returns>Validated LangfuseOptions instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown if required configuration is missing.</exception>
    public static LangfuseOptions LoadAndValidate(IConfiguration? configuration = null)
    {
        var options = Load(configuration);
        options.Validate();
        return options;
    }

    /// <summary>
    /// Merges user-provided options with configuration from environment/IConfiguration.
    /// User-provided values take precedence.
    /// </summary>
    /// <param name="userOptions">User-provided options (may have null values).</param>
    /// <param name="configuration">Optional IConfiguration instance.</param>
    /// <returns>Merged and defaulted LangfuseOptions instance.</returns>
    public static LangfuseOptions Merge(LangfuseOptions? userOptions, IConfiguration? configuration = null)
    {
        var baseOptions = Load(configuration);

        if (userOptions == null)
            return baseOptions;

        // User options override loaded options
        return new LangfuseOptions
        {
            BaseUrl = !string.IsNullOrWhiteSpace(userOptions.BaseUrl) ? userOptions.BaseUrl.TrimEnd('/') : baseOptions.BaseUrl,
            PublicKey = !string.IsNullOrWhiteSpace(userOptions.PublicKey) ? userOptions.PublicKey : baseOptions.PublicKey,
            SecretKey = !string.IsNullOrWhiteSpace(userOptions.SecretKey) ? userOptions.SecretKey : baseOptions.SecretKey,
            Timeout = userOptions.Timeout != TimeSpan.FromSeconds(30) ? userOptions.Timeout : baseOptions.Timeout
        };
    }
}

