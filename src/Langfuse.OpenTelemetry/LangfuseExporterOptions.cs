using Langfuse.Core;
using OpenTelemetry.Exporter;

namespace Langfuse.OpenTelemetry;

/// <summary>
/// Configuration options for the Langfuse OpenTelemetry exporter.
/// Extends base LangfuseOptions with OTEL-specific settings.
/// </summary>
public class LangfuseExporterOptions : LangfuseOptions
{
    /// <summary>
    /// Gets or sets the OTLP protocol to use.
    /// Default: HttpProtobuf
    /// </summary>
    public OtlpExportProtocol Protocol { get; set; } = OtlpExportProtocol.HttpProtobuf;

    /// <summary>
    /// Gets or sets additional headers to include in the export request.
    /// </summary>
    public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// Creates new options from base LangfuseOptions.
    /// </summary>
    internal static LangfuseExporterOptions FromBase(LangfuseOptions baseOptions)
    {
        return new LangfuseExporterOptions
        {
            BaseUrl = baseOptions.BaseUrl,
            PublicKey = baseOptions.PublicKey,
            SecretKey = baseOptions.SecretKey,
            Timeout = baseOptions.Timeout
        };
    }
}
