using Jha.Reddit.Shared.Entities;

namespace Jha.Reddit.Shared;

/// <summary>
/// Class TopUserChangedEventArgs.
/// Implements the <see cref="EventArgs" />
/// </summary>
/// <seealso cref="EventArgs" />
public class TopUserChangedEventArgs(RedditFeedEntity feed, RedditUserStatsEntity userStats) : EventArgs
{
    /// <summary>
    /// Gets the feed.
    /// </summary>
    /// <value>The feed.</value>
    public RedditFeedEntity Feed { get; } = feed ?? throw new ArgumentNullException(nameof(feed));

    /// <summary>
    /// Gets the post.
    /// </summary>
    /// <value>The post.</value>
    public RedditUserStatsEntity UserStats { get; } = userStats ?? throw new ArgumentNullException(nameof(userStats));
}