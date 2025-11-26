using Langfuse.Core;
using Microsoft.Extensions.Configuration;

namespace Langfuse.OpenTelemetry.Internal;

/// <summary>
/// Internal helper to load Langfuse exporter configuration from various sources.
/// Wraps Core.ConfigurationLoader with exporter-specific functionality.
/// </summary>
internal static class EnvironmentConfigLoader
{
    /// <summary>
    /// Loads configuration from environment variables and applies defaults.
    /// </summary>
    public static LangfuseExporterOptions LoadFromEnvironment(IConfiguration? configuration = null)
    {
        var baseOptions = ConfigurationLoader.Load(configuration);
        return LangfuseExporterOptions.FromBase(baseOptions);
    }

    /// <summary>
    /// Validates that required options are set.
    /// </summary>
    public static void Validate(LangfuseExporterOptions options)
    {
        options.Validate();

        // Ensure BaseUrl doesn't have trailing slash
        if (!string.IsNullOrWhiteSpace(options.BaseUrl))
        {
            options.BaseUrl = options.BaseUrl.TrimEnd('/');
        }
    }

    /// <summary>
    /// Builds the Basic Authentication header value.
    /// </summary>
    public static string BuildAuthHeader(string publicKey, string secretKey)
    {
        return AuthenticationHelper.BuildBasicAuthHeader(publicKey, secretKey);
    }

    /// <summary>
    /// Builds the OTLP traces endpoint URL.
    /// </summary>
    public static string BuildTracesEndpoint(string baseUrl)
    {
        return $"{baseUrl}{LangfuseConstants.OtelTracesPath}";
    }
}
