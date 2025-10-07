using System;
using Microsoft.Extensions.Configuration;

namespace Langfuse.OpenTelemetry.Internal
{
    /// <summary>
    /// Internal helper to load Langfuse configuration from various sources.
    /// Priority order: Code-based options > Environment variables > IConfiguration > Defaults
    /// </summary>
    internal static class EnvironmentConfigLoader
    {
        private const string DefaultBaseUrl = "https://cloud.langfuse.com";
        
        private const string EnvPublicKey = "LANGFUSE_PUBLIC_KEY";
        private const string EnvSecretKey = "LANGFUSE_SECRET_KEY";
        private const string EnvBaseUrl = "LANGFUSE_BASE_URL";

        /// <summary>
        /// Loads configuration from environment variables and applies defaults.
        /// </summary>
        public static LangfuseExporterOptions LoadFromEnvironment(IConfiguration? configuration = null)
        {
            var options = new LangfuseExporterOptions();

            // Try configuration first (appsettings.json, etc.)
            if (configuration != null)
            {
                options.BaseUrl = configuration["Langfuse:BaseUrl"];
                options.PublicKey = configuration["Langfuse:PublicKey"];
                options.SecretKey = configuration["Langfuse:SecretKey"];
            }

            // Override with environment variables
            options.BaseUrl = Environment.GetEnvironmentVariable(EnvBaseUrl) ?? options.BaseUrl;
            options.PublicKey = Environment.GetEnvironmentVariable(EnvPublicKey) ?? options.PublicKey;
            options.SecretKey = Environment.GetEnvironmentVariable(EnvSecretKey) ?? options.SecretKey;

            // Apply defaults
            options.BaseUrl ??= DefaultBaseUrl;

            return options;
        }

        /// <summary>
        /// Validates that required options are set.
        /// </summary>
        public static void Validate(LangfuseExporterOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.BaseUrl))
            {
                throw new InvalidOperationException(
                    $"Langfuse BaseUrl is required. Set it via code, environment variable ({EnvBaseUrl}), or configuration.");
            }

            if (string.IsNullOrWhiteSpace(options.PublicKey))
            {
                throw new InvalidOperationException(
                    $"Langfuse PublicKey is required. Set it via code, environment variable ({EnvPublicKey}), or configuration.");
            }

            if (string.IsNullOrWhiteSpace(options.SecretKey))
            {
                throw new InvalidOperationException(
                    $"Langfuse SecretKey is required. Set it via code, environment variable ({EnvSecretKey}), or configuration.");
            }

            // Ensure BaseUrl doesn't have trailing slash
            options.BaseUrl = options.BaseUrl.TrimEnd('/');
        }

        /// <summary>
        /// Builds the Basic Authentication header value.
        /// </summary>
        public static string BuildAuthHeader(string publicKey, string secretKey)
        {
            var credentials = $"{publicKey}:{secretKey}";
            var encoded = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials));
            return $"Basic {encoded}";
        }

        /// <summary>
        /// Builds the OTLP traces endpoint URL.
        /// </summary>
        public static string BuildTracesEndpoint(string baseUrl)
        {
            return $"{baseUrl}/api/public/otel/v1/traces";
        }
    }
}