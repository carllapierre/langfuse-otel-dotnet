using Langfuse.Client.Prompts;
using Xunit;

namespace Langfuse.Client.Tests.Prompts;

public class ChatPromptTests
{
    [Fact]
    public void Compile_ReplacesVariablesInAllMessages()
    {
        var prompt = CreateChatPrompt(
            PromptMessage.System("You are a {{role}} assistant."),
            PromptMessage.User("Help me with {{task}}.")
        );

        var result = prompt.Compile(new Dictionary<string, string>
        {
            ["role"] = "helpful",
            ["task"] = "coding"
        });

        Assert.Equal(2, result.Count);
        Assert.Equal("You are a helpful assistant.", result[0].Content);
        Assert.Equal("Help me with coding.", result[1].Content);
    }

    [Fact]
    public void Compile_PreservesRoles()
    {
        var prompt = CreateChatPrompt(
            PromptMessage.System("System content"),
            PromptMessage.User("User content"),
            PromptMessage.Assistant("Assistant content")
        );

        var result = prompt.Compile();

        Assert.Equal("system", result[0].Role);
        Assert.Equal("user", result[1].Role);
        Assert.Equal("assistant", result[2].Role);
    }

    [Fact]
    public void Compile_WithNullVariables_ReturnsOriginalMessages()
    {
        var prompt = CreateChatPrompt(
            PromptMessage.User("Hello {{name}}!")
        );

        var result = prompt.Compile((IDictionary<string, string>?)null);

        Assert.Single(result);
        Assert.Equal("Hello {{name}}!", result[0].Content);
    }

    [Fact]
    public void Compile_WithTuples_Works()
    {
        var prompt = CreateChatPrompt(
            PromptMessage.User("Hello {{name}}!")
        );

        var result = prompt.Compile(("name", "World"));

        Assert.Equal("Hello World!", result[0].Content);
    }

    [Fact]
    public void CreateFallback_SetsIsFallbackTrue()
    {
        var messages = new[] { PromptMessage.User("Fallback message") };
        var prompt = ChatPrompt.CreateFallback("test-prompt", messages);

        Assert.True(prompt.IsFallback);
        Assert.Equal("test-prompt", prompt.Name);
        Assert.Single(prompt.Prompt);
        Assert.Equal(0, prompt.Version);
    }

    private static ChatPrompt CreateChatPrompt(params PromptMessage[] messages)
    {
        // Create using the fallback method for testing
        return ChatPrompt.CreateFallback("test-prompt", messages);
    }
}

