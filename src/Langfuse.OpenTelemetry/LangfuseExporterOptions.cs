using System;
using System.Collections.Generic;
using OpenTelemetry.Exporter;

namespace Langfuse.OpenTelemetry
{
    /// <summary>
    /// Configuration options for the Langfuse OpenTelemetry exporter.
    /// </summary>
    public class LangfuseExporterOptions
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
        /// Gets or sets the OTLP protocol to use.
        /// Default: HttpProtobuf
        /// </summary>
        public OtlpExportProtocol Protocol { get; set; } = OtlpExportProtocol.HttpProtobuf;

        /// <summary>
        /// Gets or sets the export timeout.
        /// Default: 30 seconds
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Gets or sets additional headers to include in the export request.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
    }
}
