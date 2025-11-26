using Langfuse.Client.Prompts;
using Xunit;

namespace Langfuse.Client.Tests.Prompts;

public class TextPromptTests
{
    [Fact]
    public void Compile_ReplacesVariables()
    {
        var prompt = CreateTextPrompt("Hello {{name}}, welcome to {{place}}!");

        var result = prompt.Compile(new Dictionary<string, string>
        {
            ["name"] = "World",
            ["place"] = "Langfuse"
        });

        Assert.Equal("Hello World, welcome to Langfuse!", result);
    }

    [Fact]
    public void Compile_KeepsUnmatchedVariables()
    {
        var prompt = CreateTextPrompt("Hello {{name}}, you have {{count}} messages.");

        var result = prompt.Compile(new Dictionary<string, string>
        {
            ["name"] = "World"
        });

        Assert.Equal("Hello World, you have {{count}} messages.", result);
    }

    [Fact]
    public void Compile_WithNullVariables_ReturnsOriginal()
    {
        var prompt = CreateTextPrompt("Hello {{name}}!");

        var result = prompt.Compile((IDictionary<string, string>?)null);

        Assert.Equal("Hello {{name}}!", result);
    }

    [Fact]
    public void Compile_WithTuples_Works()
    {
        var prompt = CreateTextPrompt("Hello {{name}}!");

        var result = prompt.Compile(("name", "World"));

        Assert.Equal("Hello World!", result);
    }

    [Fact]
    public void CreateFallback_SetsIsFallbackTrue()
    {
        var prompt = TextPrompt.CreateFallback("test-prompt", "fallback content");

        Assert.True(prompt.IsFallback);
        Assert.Equal("test-prompt", prompt.Name);
        Assert.Equal("fallback content", prompt.Prompt);
        Assert.Equal(0, prompt.Version);
    }

    private static TextPrompt CreateTextPrompt(string template)
    {
        // Create using the fallback method for testing
        return TextPrompt.CreateFallback("test-prompt", template);
    }
}

