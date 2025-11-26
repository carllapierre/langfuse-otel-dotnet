using Langfuse.Core;
using Xunit;

namespace Langfuse.Core.Tests;

public class LangfuseOptionsTests
{
    [Fact]
    public void WithDefaults_AppliesDefaultBaseUrl()
    {
        var options = new LangfuseOptions
        {
            PublicKey = "pk-test",
            SecretKey = "sk-test"
        };

        var result = options.WithDefaults();

        Assert.Equal(LangfuseConstants.DefaultBaseUrl, result.BaseUrl);
    }

    [Fact]
    public void WithDefaults_PreservesCustomBaseUrl()
    {
        var options = new LangfuseOptions
        {
            BaseUrl = "https://custom.langfuse.com",
            PublicKey = "pk-test",
            SecretKey = "sk-test"
        };

        var result = options.WithDefaults();

        Assert.Equal("https://custom.langfuse.com", result.BaseUrl);
    }

    [Fact]
    public void WithDefaults_TrimsTrailingSlash()
    {
        var options = new LangfuseOptions
        {
            BaseUrl = "https://custom.langfuse.com/",
            PublicKey = "pk-test",
            SecretKey = "sk-test"
        };

        var result = options.WithDefaults();

        Assert.Equal("https://custom.langfuse.com", result.BaseUrl);
    }

    [Fact]
    public void Validate_ThrowsWhenPublicKeyMissing()
    {
        var options = new LangfuseOptions
        {
            SecretKey = "sk-test"
        };

        var exception = Assert.Throws<InvalidOperationException>(() => options.Validate());
        Assert.Contains("PublicKey", exception.Message);
    }

    [Fact]
    public void Validate_ThrowsWhenSecretKeyMissing()
    {
        var options = new LangfuseOptions
        {
            PublicKey = "pk-test"
        };

        var exception = Assert.Throws<InvalidOperationException>(() => options.Validate());
        Assert.Contains("SecretKey", exception.Message);
    }

    [Fact]
    public void Validate_SucceedsWithRequiredValues()
    {
        var options = new LangfuseOptions
        {
            PublicKey = "pk-test",
            SecretKey = "sk-test"
        };

        var exception = Record.Exception(() => options.Validate());

        Assert.Null(exception);
    }

    [Fact]
    public void DefaultTimeout_Is30Seconds()
    {
        var options = new LangfuseOptions();

        Assert.Equal(TimeSpan.FromSeconds(30), options.Timeout);
    }
}

