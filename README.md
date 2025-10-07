# .NET Langfuse OTEL Exporter

Export .NET OpenTelemetry traces to Langfuse with one line of code.

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

**Links:**
- [Langfuse OpenTelemetry Docs](https://langfuse.com/docs/opentelemetry)
- [OpenTelemetry GenAI Conventions](https://github.com/open-telemetry/semantic-conventions/tree/main/docs/gen-ai)
- [Semantic Kernel](https://github.com/microsoft/semantic-kernel)
