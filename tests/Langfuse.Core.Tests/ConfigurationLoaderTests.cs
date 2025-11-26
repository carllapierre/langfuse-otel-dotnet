using Langfuse.Core;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Langfuse.Core.Tests;

public class ConfigurationLoaderTests
{
    [Fact]
    public void Load_AppliesDefaultBaseUrl()
    {
        var options = ConfigurationLoader.Load();

        Assert.Equal(LangfuseConstants.DefaultBaseUrl, options.BaseUrl);
    }

    [Fact]
    public void Load_ReadsFromConfiguration()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Langfuse:BaseUrl"] = "https://custom.langfuse.com",
                ["Langfuse:PublicKey"] = "pk-config",
                ["Langfuse:SecretKey"] = "sk-config"
            })
            .Build();

        var options = ConfigurationLoader.Load(config);

        Assert.Equal("https://custom.langfuse.com", options.BaseUrl);
        Assert.Equal("pk-config", options.PublicKey);
        Assert.Equal("sk-config", options.SecretKey);
    }

    [Fact]
    public void Merge_UserOptionsTakePrecedence()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Langfuse:PublicKey"] = "pk-config",
                ["Langfuse:SecretKey"] = "sk-config"
            })
            .Build();

        var userOptions = new LangfuseOptions
        {
            PublicKey = "pk-user"
        };

        var options = ConfigurationLoader.Merge(userOptions, config);

        Assert.Equal("pk-user", options.PublicKey);
        Assert.Equal("sk-config", options.SecretKey);
    }

    [Fact]
    public void LoadAndValidate_ThrowsWhenKeysMissing()
    {
        Assert.Throws<InvalidOperationException>(() =>
            ConfigurationLoader.LoadAndValidate());
    }
}

