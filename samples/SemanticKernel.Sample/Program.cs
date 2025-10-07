using System;
using System.Threading.Tasks;
using Langfuse.OpenTelemetry;
using Microsoft.SemanticKernel;
using OpenTelemetry;
using OpenTelemetry.Trace;

// Check environment variables
var openAiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
var langfusePublicKey = Environment.GetEnvironmentVariable("LANGFUSE_PUBLIC_KEY");
var langfuseSecretKey = Environment.GetEnvironmentVariable("LANGFUSE_SECRET_KEY");
var langfuseBaseUrl = Environment.GetEnvironmentVariable("LANGFUSE_BASE_URL") ?? "https://cloud.langfuse.com";

if (string.IsNullOrEmpty(openAiApiKey) || string.IsNullOrEmpty(langfusePublicKey) || string.IsNullOrEmpty(langfuseSecretKey))
{
    Console.WriteLine("Missing environment variables:");
    Console.WriteLine("  OPENAI_API_KEY");
    Console.WriteLine("  LANGFUSE_PUBLIC_KEY");
    Console.WriteLine("  LANGFUSE_SECRET_KEY");
    Console.WriteLine("  LANGFUSE_BASE_URL (optional)");
    return;
}

// Enable GenAI diagnostics
AppContext.SetSwitch("Microsoft.SemanticKernel.Experimental.GenAI.EnableOTelDiagnosticsSensitive", true);

// Setup OpenTelemetry
using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddSource("Microsoft.SemanticKernel*")
    .AddLangfuseExporter()
    .Build();

// Create Semantic Kernel
var builder = Kernel.CreateBuilder();
builder.AddOpenAIChatCompletion("gpt-4o-mini", openAiApiKey);
var kernel = builder.Build();

// Run a simple prompt
var result = await kernel.InvokePromptAsync("What is OpenTelemetry?");

Console.WriteLine($"Response: {result}");
Console.WriteLine($"\nCheck your Langfuse dashboard: {langfuseBaseUrl}");

// Wait for export
await Task.Delay(1000);
