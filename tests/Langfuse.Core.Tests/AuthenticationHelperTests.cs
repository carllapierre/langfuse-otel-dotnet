using Langfuse.Core;
using Xunit;

namespace Langfuse.Core.Tests;

public class AuthenticationHelperTests
{
    [Fact]
    public void BuildBasicAuthHeader_CreatesValidHeader()
    {
        var header = AuthenticationHelper.BuildBasicAuthHeader("pk-test", "sk-test");

        Assert.StartsWith("Basic ", header);

        // Decode and verify
        var base64 = header["Basic ".Length..];
        var decoded = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        Assert.Equal("pk-test:sk-test", decoded);
    }

    [Fact]
    public void BuildBasicAuthHeader_WithOptions_CreatesValidHeader()
    {
        var options = new LangfuseOptions
        {
            PublicKey = "pk-test",
            SecretKey = "sk-test"
        };

        var header = AuthenticationHelper.BuildBasicAuthHeader(options);

        Assert.StartsWith("Basic ", header);
    }

    [Fact]
    public void BuildBasicAuthHeader_WithOptions_ThrowsWhenKeysMissing()
    {
        var options = new LangfuseOptions();

        Assert.Throws<InvalidOperationException>(() =>
            AuthenticationHelper.BuildBasicAuthHeader(options));
    }
}

