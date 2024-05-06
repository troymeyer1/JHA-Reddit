using System.ComponentModel.DataAnnotations;

namespace Jha.Reddit.Shared.Entities;

/// <summary>
/// Class RedditFeedEntity.
/// </summary>
public class RedditFeedEntity
{
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the sub reddit.
    /// </summary>
    /// <value>The sub reddit.</value>
    [Required]
    public string SubReddit { get; set; }
}