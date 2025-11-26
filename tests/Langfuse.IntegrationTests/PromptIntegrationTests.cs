using Langfuse.Client;
using Langfuse.Client.Prompts;
using Langfuse.Core;
using Xunit;

namespace Langfuse.IntegrationTests;

/// <summary>
/// Integration tests for Langfuse prompts.
/// Runs against Cloud (if LANGFUSE_PUBLIC_KEY/SECRET_KEY are set) or Local Docker instance.
/// </summary>
[Trait("Category", "Integration")]
public class PromptIntegrationTests : IDisposable
{
    private readonly LangfuseClient? _client;
    private readonly bool _skipTests;
    private readonly string _environment;

    public PromptIntegrationTests()
    {
        // Try Cloud first, then Local
        if (TestConfiguration.IsCloudConfigured())
        {
            _environment = "Cloud";
            var options = TestConfiguration.GetCloudOptions();
            _client = new LangfuseClient(new LangfuseClientOptions
            {
                BaseUrl = options.BaseUrl,
                PublicKey = options.PublicKey,
                SecretKey = options.SecretKey
            });
        }
        else if (TestConfiguration.IsLocalConfigured())
        {
            _environment = "Local";
            var options = TestConfiguration.GetLocalOptions();
            _client = new LangfuseClient(new LangfuseClientOptions
            {
                BaseUrl = options.BaseUrl,
                PublicKey = options.PublicKey,
                SecretKey = options.SecretKey,
                Timeout = TimeSpan.FromSeconds(10)
            });
        }
        else
        {
            _skipTests = true;
            _environment = "None";
        }
    }

    [SkippableFact]
    public async Task GetPrompt_WithValidName_ReturnsPrompt()
    {
        Skip.If(_skipTests, "No Langfuse configuration found. Set LANGFUSE_PUBLIC_KEY/SECRET_KEY for cloud, or LANGFUSE_LOCAL_PUBLIC_KEY/SECRET_KEY for local.");

        var promptName = Environment.GetEnvironmentVariable("LANGFUSE_TEST_PROMPT_NAME") ?? "test-prompt";

        try
        {
            var prompt = await _client!.GetPromptAsync(promptName);

            Assert.NotNull(prompt);
            Assert.Equal(promptName, prompt.Name);
            Assert.False(prompt.IsFallback);
        }
        catch (LangfuseApiException ex) when (ex.StatusCode == 404)
        {
            Skip.If(true, $"Test prompt '{promptName}' not found in {_environment}. Create it or set LANGFUSE_TEST_PROMPT_NAME.");
        }
        catch (LangfuseApiException ex) when (ex.StatusCode == 401)
        {
            Assert.Fail($"Authentication failed against {_environment}. Check your API keys.");
        }
        // Let HttpRequestException propagate - test should FAIL if can't connect
    }

    [SkippableFact]
    public async Task GetPrompt_WithInvalidName_ThrowsNotFoundException()
    {
        Skip.If(_skipTests, "No Langfuse configuration found.");

        var exception = await Assert.ThrowsAsync<LangfuseApiException>(() =>
            _client!.GetPromptAsync("non-existent-prompt-" + Guid.NewGuid()));

        Assert.Equal(404, exception.StatusCode);
    }

    [SkippableFact]
    public async Task GetPrompt_WithFallback_ReturnsFallbackOnNotFound()
    {
        Skip.If(_skipTests, "No Langfuse configuration found.");

        var fallback = TextPrompt.CreateFallback("fallback", "Fallback content");

        var prompt = await _client!.GetPromptAsync(
            "non-existent-prompt-" + Guid.NewGuid(),
            fallback: fallback);

        Assert.True(prompt.IsFallback);
        Assert.Equal("Fallback content", prompt.Prompt);
    }

    [SkippableFact]
    public async Task GetChatPrompt_WithFallback_ReturnsFallbackOnNotFound()
    {
        Skip.If(_skipTests, "No Langfuse configuration found.");

        var messages = new[]
        {
            new PromptMessage("system", "You are a helpful assistant."),
            new PromptMessage("user", "Hello!")
        };
        var fallback = ChatPrompt.CreateFallback("chat-fallback", messages);

        var prompt = await _client!.GetChatPromptAsync(
            "non-existent-chat-prompt-" + Guid.NewGuid(),
            fallback: fallback);

        Assert.True(prompt.IsFallback);
        Assert.Equal(2, prompt.Prompt.Count);
    }

    public void Dispose()
    {
        _client?.Dispose();
    }
}

