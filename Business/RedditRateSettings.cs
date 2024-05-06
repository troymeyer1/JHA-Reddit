namespace Jha.Reddit.Business;

/// <summary>
/// Class RedditRateSettings. This class cannot be inherited.
/// Implements the <see cref="IEquatable{RedditRateSettings}" />
/// </summary>
/// <seealso cref="IEquatable{RedditRateSettings}" />
public sealed record RedditRateSettings(int RateLimit, int RateLimitRemaining, int RateLimitReset);