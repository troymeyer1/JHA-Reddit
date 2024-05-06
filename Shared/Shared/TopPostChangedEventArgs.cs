using Jha.Reddit.Shared.Entities;

namespace Jha.Reddit.Shared;

/// <summary>
/// Class TopPostChangedEventArgs.
/// Implements the <see cref="EventArgs" />
/// </summary>
/// <seealso cref="EventArgs" />
/// <remarks>Initializes a new instance of the <see cref="TopPostChangedEventArgs" /> class.</remarks>
public class TopPostChangedEventArgs(RedditFeedEntity feed, RedditPostEntity post) : EventArgs
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
    public RedditPostEntity Post { get; } = post ?? throw new ArgumentNullException(nameof(post));
}