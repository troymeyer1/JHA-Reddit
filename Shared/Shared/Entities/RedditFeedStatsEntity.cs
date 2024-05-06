namespace Jha.Reddit.Shared.Entities;

/// <summary>
/// Class RedditFeedStatsEntity.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="RedditFeedStatsEntity"/> class.
/// </remarks>
/// <param name="feed">The feed.</param>
/// <exception cref="ArgumentNullException">feed</exception>
public class RedditFeedStatsEntity(RedditFeedEntity feed)
{
    /// <summary>
    /// Gets the feed.
    /// </summary>
    /// <value>The feed.</value>
    public RedditFeedEntity Feed { get; } = feed ?? throw new ArgumentNullException(nameof(feed));

    /// <summary>
    /// Gets or sets the top post.
    /// </summary>
    /// <value>The top post.</value>
    public RedditPostEntity TopPost { get; set; }

    /// <summary>
    /// Gets or sets the top user.
    /// </summary>
    /// <value>The top user.</value>
    public RedditUserStatsEntity TopUser { get; set; }
}