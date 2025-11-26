namespace Langfuse.Core;

/// <summary>
/// Exception thrown when a Langfuse API request fails.
/// </summary>
public class LangfuseApiException : Exception
{
    /// <summary>
    /// Gets the HTTP status code of the failed request.
    /// </summary>
    public int StatusCode { get; }

    /// <summary>
    /// Gets the raw response body from the API, if available.
    /// </summary>
    public string? ResponseBody { get; }

    /// <summary>
    /// Creates a new instance of LangfuseApiException.
    /// </summary>
    /// <param name="message">Error message.</param>
    /// <param name="statusCode">HTTP status code.</param>
    /// <param name="responseBody">Optional response body.</param>
    public LangfuseApiException(string message, int statusCode, string? responseBody = null)
        : base(message)
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
    }

    /// <summary>
    /// Creates a new instance of LangfuseApiException with an inner exception.
    /// </summary>
    /// <param name="message">Error message.</param>
    /// <param name="statusCode">HTTP status code.</param>
    /// <param name="innerException">Inner exception.</param>
    public LangfuseApiException(string message, int statusCode, Exception innerException)
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }
}

