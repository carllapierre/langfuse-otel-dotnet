# Implemented Features

This document lists the Langfuse features implemented in this unofficial .NET SDK, with links to the official Langfuse documentation.

## Overview

| Package | Description | Status |
|---------|-------------|--------|
| `Langfuse.Core` | Shared configuration, authentication, HTTP utilities | ✅ Stable |
| `Langfuse.Client` | API client for Langfuse features | ✅ Stable |
| `Langfuse.OpenTelemetry` | OTEL trace exporter to Langfuse | ✅ Stable |

---

## Langfuse.OpenTelemetry

### OpenTelemetry Trace Export

Export .NET OpenTelemetry traces to Langfuse for LLM observability.

**Langfuse Docs:**
- [OpenTelemetry Integration](https://langfuse.com/docs/integrations/otel)
- [Observability Overview](https://langfuse.com/docs/observability/overview)

**Usage:**

```csharp
using Langfuse.OpenTelemetry;
using OpenTelemetry.Trace;

var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddSource("MyApp")
    .AddLangfuseExporter()  // Uses LANGFUSE_* env vars
    .Build();
```

**Configuration Options:**
- `BaseUrl` - Langfuse instance URL
- `PublicKey` - API public key
- `SecretKey` - API secret key
- `Protocol` - OTLP protocol (HttpProtobuf default)
- `Timeout` - Request timeout

---

## Langfuse.Client

### Prompt Management

Fetch and use prompts managed in Langfuse with client-side caching.

**Langfuse Docs:**
- [Prompt Management Overview](https://langfuse.com/docs/prompt-management/overview)
- [Get Started with Prompts](https://langfuse.com/docs/prompt-management/get-started)
- [Prompt Version Control](https://langfuse.com/docs/prompt-management/features/prompt-version-control)
- [Caching](https://langfuse.com/docs/prompt-management/features/caching)

**Features Implemented:**

| Feature | Status | Notes |
|---------|--------|-------|
| Fetch text prompts | ✅ | `GetPromptAsync()` |
| Fetch chat prompts | ✅ | `GetChatPromptAsync()` |
| Variable compilation | ✅ | `prompt.Compile()` with `{{variable}}` syntax |
| Version selection | ✅ | `version` parameter |
| Label selection | ✅ | `label` parameter (production, staging, etc.) |
| Client-side caching | ✅ | 60s TTL (configurable) |
| Fallback prompts | ✅ | Use fallback when API fails |
| Config access | ✅ | `prompt.Config`, `GetConfigValue<T>()` |

**Usage:**

```csharp
using Langfuse.Client;

var client = new LangfuseClient();

// Text prompt
var prompt = await client.GetPromptAsync("movie-critic");
var compiled = prompt.Compile(new Dictionary<string, string>
{
    ["criticlevel"] = "expert",
    ["movie"] = "Dune 2"
});

// Chat prompt
var chatPrompt = await client.GetChatPromptAsync("movie-critic-chat");
var messages = chatPrompt.Compile(("criticlevel", "expert"), ("movie", "Dune 2"));

// With version/label
var v1 = await client.GetPromptAsync("my-prompt", version: 1);
var staging = await client.GetPromptAsync("my-prompt", label: "staging");

// With fallback
var fallback = TextPrompt.CreateFallback("default", "Default prompt text");
var safe = await client.GetPromptAsync("my-prompt", fallback: fallback);
```

---

## Not Yet Implemented

The following Langfuse features are not yet implemented in this SDK:

### Observability (via Client SDK)
- [ ] Manual trace/span creation via SDK
- [ ] Score/evaluation submission
- [ ] User feedback collection

**Langfuse Docs:** [SDK Tracing](https://langfuse.com/docs/observability/sdk/overview)

### Datasets & Evaluation
- [ ] Dataset management
- [ ] Dataset item creation
- [ ] Experiment runs

**Langfuse Docs:** [Datasets](https://langfuse.com/docs/evaluation/experiments/datasets)

### Scores
- [ ] Create scores via SDK
- [ ] Score configurations

**Langfuse Docs:** [Scores](https://langfuse.com/docs/evaluation/evaluation-methods/custom-scores)

### API Query
- [ ] Query traces via SDK
- [ ] Query observations
- [ ] Query sessions

**Langfuse Docs:** [Query via SDK](https://langfuse.com/docs/api-and-data-platform/features/query-via-sdk)

---

## API Reference

### Langfuse Public API

This SDK uses the Langfuse Public API. Full API documentation:

- [API Reference](https://api.reference.langfuse.com)
- [OpenAPI Spec](https://cloud.langfuse.com/generated/api/openapi.yml)
- [Authentication](https://langfuse.com/docs/api-and-data-platform/features/public-api#authentication)

---

## Contributing

Want to help implement missing features? See [CONTRIBUTING.md](../CONTRIBUTING.md) for guidelines.

