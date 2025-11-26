using Langfuse.Client.Prompts;
using Xunit;

namespace Langfuse.Client.Tests.Prompts;

public class PromptMessageTests
{
    [Fact]
    public void System_CreatesSystemMessage()
    {
        var message = PromptMessage.System("You are a helpful assistant.");

        Assert.Equal("system", message.Role);
        Assert.Equal("You are a helpful assistant.", message.Content);
    }

    [Fact]
    public void User_CreatesUserMessage()
    {
        var message = PromptMessage.User("Hello, world!");

        Assert.Equal("user", message.Role);
        Assert.Equal("Hello, world!", message.Content);
    }

    [Fact]
    public void Assistant_CreatesAssistantMessage()
    {
        var message = PromptMessage.Assistant("How can I help?");

        Assert.Equal("assistant", message.Role);
        Assert.Equal("How can I help?", message.Content);
    }

    [Fact]
    public void Constructor_SetsProperties()
    {
        var message = new PromptMessage("custom", "Custom content");

        Assert.Equal("custom", message.Role);
        Assert.Equal("Custom content", message.Content);
    }
}

