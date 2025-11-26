using Langfuse.Client.Prompts;
using Microsoft.Extensions.Caching.Memory;

namespace Langfuse.Client.Caching;

/// <summary>
/// Cache implementation for Langfuse prompts with TTL support.
/// </summary>
internal class PromptCache : IDisposable
{
    private readonly MemoryCache _cache;
    private readonly TimeSpan _defaultTtl;
    private bool _disposed;

    public PromptCache(TimeSpan defaultTtl)
    {
        _defaultTtl = defaultTtl;
        _cache = new MemoryCache(new MemoryCacheOptions
        {
            SizeLimit = 1000 // Maximum number of cached prompts
        });
    }

    /// <summary>
    /// Gets the cache key for a prompt request.
    /// </summary>
    public static string GetCacheKey(string name, int? version, string? label)
    {
        return $"prompt:{name}:v{version?.ToString() ?? "null"}:l{label ?? "null"}";
    }

    /// <summary>
    /// Tries to get a cached text prompt.
    /// </summary>
    public bool TryGetTextPrompt(string cacheKey, out TextPrompt? prompt)
    {
        return _cache.TryGetValue(cacheKey, out prompt);
    }

    /// <summary>
    /// Tries to get a cached chat prompt.
    /// </summary>
    public bool TryGetChatPrompt(string cacheKey, out ChatPrompt? prompt)
    {
        return _cache.TryGetValue(cacheKey, out prompt);
    }

    /// <summary>
    /// Caches a text prompt.
    /// </summary>
    public void SetTextPrompt(string cacheKey, TextPrompt prompt, TimeSpan? ttl = null)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = ttl ?? _defaultTtl,
            Size = 1
        };

        _cache.Set(cacheKey, prompt, options);
    }

    /// <summary>
    /// Caches a chat prompt.
    /// </summary>
    public void SetChatPrompt(string cacheKey, ChatPrompt prompt, TimeSpan? ttl = null)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = ttl ?? _defaultTtl,
            Size = 1
        };

        _cache.Set(cacheKey, prompt, options);
    }

    /// <summary>
    /// Removes a specific prompt from cache.
    /// </summary>
    public void Remove(string cacheKey)
    {
        _cache.Remove(cacheKey);
    }

    /// <summary>
    /// Clears all cached prompts.
    /// </summary>
    public void Clear()
    {
        _cache.Compact(1.0); // Remove all entries
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _cache.Dispose();
        _disposed = true;
    }
}

