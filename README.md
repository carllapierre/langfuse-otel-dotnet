# .NET Langfuse OTEL Exporter

[![NuGet](https://img.shields.io/nuget/v/Langfuse.OpenTelemetry.svg)](https://www.nuget.org/packages/Langfuse.OpenTelemetry/)
[![Build](https://github.com/carllapierre/langfuse-otel-dotnet/actions/workflows/build.yml/badge.svg)](https://github.com/carllapierre/langfuse-otel-dotnet/actions/workflows/build.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Export .NET OpenTelemetry traces to Langfuse with one line of code. Agentic Frameworks like Semantic Kernel are instrumented to emit OpenTelemetry and this library aims to collect OTEL data and export it to LangFuse's OTEL endpoints. 

## Quick Start

**1. Install**
```bash
dotnet add package Langfuse.OpenTelemetry
```

**2. Set environment variables**
```bash
LANGFUSE_PUBLIC_KEY=pk-lf-...
LANGFUSE_SECRET_KEY=sk-lf-...
LANGFUSE_BASE_URL=https://cloud.langfuse.com
```

**3. Add to your app**
```csharp
using Langfuse.OpenTelemetry;
using Microsoft.SemanticKernel;
using OpenTelemetry;
using OpenTelemetry.Trace;

// Enable GenAI diagnostics (prompts, tokens, completions)
AppContext.SetSwitch("Microsoft.SemanticKernel.Experimental.GenAI.EnableOTelDiagnosticsSensitive", true);

// Setup OpenTelemetry
using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddSource("Microsoft.SemanticKernel*")
    .AddLangfuseExporter()
    .Build();

// Use Semantic Kernel as normal
var kernel = Kernel.CreateBuilder()
    .AddOpenAIChatCompletion("gpt-4o-mini", apiKey)
    .Build();

var result = await kernel.InvokePromptAsync("Hello!");
```

## Test

```bash
# Set environment variables
$env:OPENAI_API_KEY="sk-..."
$env:LANGFUSE_PUBLIC_KEY="pk-lf-..."
$env:LANGFUSE_SECRET_KEY="sk-lf-..."
$env:LANGFUSE_BASE_URL="https://cloud.langfuse.com"

# Run sample
cd samples/SemanticKernel.Sample
dotnet run
```

Check your Langfuse dashboard to see the traces.

---

## Contributing

Contributions are welcome! Please see [CONTRIBUTING.md](CONTRIBUTING.md) for details.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Links

- [Langfuse OpenTelemetry Docs](https://langfuse.com/docs/opentelemetry)
- [OpenTelemetry GenAI Conventions](https://github.com/open-telemetry/semantic-conventions/tree/main/docs/gen-ai)
- [Semantic Kernel](https://github.com/microsoft/semantic-kernel)
