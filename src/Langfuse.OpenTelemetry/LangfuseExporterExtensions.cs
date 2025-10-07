using System;
using Langfuse.OpenTelemetry.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace;

namespace Langfuse.OpenTelemetry
{
    /// <summary>
    /// Extension methods for adding Langfuse exporter to OpenTelemetry TracerProviderBuilder.
    /// </summary>
    public static class LangfuseExporterExtensions
    {
        /// <summary>
        /// Adds Langfuse OTLP exporter to the TracerProviderBuilder.
        /// Configuration is loaded from environment variables and/or IConfiguration.
        /// </summary>
        /// <param name="builder">The TracerProviderBuilder to configure.</param>
        /// <returns>The TracerProviderBuilder for chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown if required configuration is missing.</exception>
        public static TracerProviderBuilder AddLangfuseExporter(this TracerProviderBuilder builder)
        {
            return AddLangfuseExporter(builder, configure: null);
        }

        /// <summary>
        /// Adds Langfuse OTLP exporter to the TracerProviderBuilder with custom configuration.
        /// </summary>
        /// <param name="builder">The TracerProviderBuilder to configure.</param>
        /// <param name="configure">Action to configure LangfuseExporterOptions.</param>
        /// <returns>The TracerProviderBuilder for chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown if required configuration is missing.</exception>
        public static TracerProviderBuilder AddLangfuseExporter(
            this TracerProviderBuilder builder,
            Action<LangfuseExporterOptions>? configure)
        {
            // Load from environment first
            var options = EnvironmentConfigLoader.LoadFromEnvironment();

            // Apply user configuration
            configure?.Invoke(options);

            // Validate configuration
            EnvironmentConfigLoader.Validate(options);

            // Build endpoint and auth header
            var tracesEndpoint = EnvironmentConfigLoader.BuildTracesEndpoint(options.BaseUrl!);
            var authHeader = EnvironmentConfigLoader.BuildAuthHeader(options.PublicKey!, options.SecretKey!);

            // Add OTLP exporter with Langfuse-specific configuration
            builder.AddOtlpExporter(otlpOptions =>
            {
                otlpOptions.Endpoint = new Uri(tracesEndpoint);
                otlpOptions.Protocol = options.Protocol;
                otlpOptions.TimeoutMilliseconds = (int)options.Timeout.TotalMilliseconds;
                
                // Set Authorization header
                otlpOptions.Headers = $"Authorization={authHeader}";
                
                // Add any additional headers
                if (options.Headers.Count > 0)
                {
                    var headerString = otlpOptions.Headers ?? "";
                    foreach (var header in options.Headers)
                    {
                        if (!string.IsNullOrEmpty(headerString))
                        {
                            headerString += ",";
                        }
                        headerString += $"{header.Key}={header.Value}";
                    }
                    otlpOptions.Headers = headerString;
                }
            });

            return builder;
        }

        /// <summary>
        /// Adds Langfuse OTLP exporter to the TracerProviderBuilder using IConfiguration.
        /// </summary>
        /// <param name="builder">The TracerProviderBuilder to configure.</param>
        /// <param name="configuration">The IConfiguration to load settings from.</param>
        /// <returns>The TracerProviderBuilder for chaining.</returns>
        public static TracerProviderBuilder AddLangfuseExporter(
            this TracerProviderBuilder builder,
            IConfiguration configuration)
        {
            var options = EnvironmentConfigLoader.LoadFromEnvironment(configuration);
            EnvironmentConfigLoader.Validate(options);

            var tracesEndpoint = EnvironmentConfigLoader.BuildTracesEndpoint(options.BaseUrl!);
            var authHeader = EnvironmentConfigLoader.BuildAuthHeader(options.PublicKey!, options.SecretKey!);

            builder.AddOtlpExporter(otlpOptions =>
            {
                otlpOptions.Endpoint = new Uri(tracesEndpoint);
                otlpOptions.Protocol = options.Protocol;
                otlpOptions.TimeoutMilliseconds = (int)options.Timeout.TotalMilliseconds;
                otlpOptions.Headers = $"Authorization={authHeader}";
            });

            return builder;
        }
    }
}