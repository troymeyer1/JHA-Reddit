namespace Jha.Reddit.Business;

/// <summary>
/// Class HttpHeaders.
/// </summary>
public static class HttpHeaders
{
    /// <summary>
    /// The rate limit used
    /// </summary>
    public const string RateLimitUsed = "X-Ratelimit-Used";
    
    /// <summary>
    /// The rate limit remaining
    /// </summary>
    public const string RateLimitRemaining = "X-Ratelimit-Remaining";
    
    /// <summary>
    /// The rate limit reset
    /// </summary>
    public const string RateLimitReset = "X-Ratelimit-Reset";
    
    /// <summary>
    /// The rate limit sleep
    /// </summary>
    public const string RateLimitSleep = "X-Ratelimit-Sleep";
}