
namespace Jha.Reddit.Shared.Entities;

/// <summary>
/// Class RedditUserStatsEntity.
/// Implements the <see cref="IEquatable{RedditUserStatsEntity}" />
/// </summary>
/// <seealso cref="IEquatable{RedditUserStatsEntity}" />
public record RedditUserStatsEntity
{
    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    /// <value>The name of the user.</value>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the post count.
    /// </summary>
    /// <value>The post count.</value>
    public int PostCount { get; set; }
}