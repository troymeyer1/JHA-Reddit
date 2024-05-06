namespace Jha.Reddit.Shared.Entities;

public record RedditPostEntity
{
    /// <summary>
    /// Gets or sets the URL.
    /// </summary>
    /// <value>The URL.</value>
    public string Url => !string.IsNullOrEmpty(PermaLink) ? $"https://www.reddit.com{PermaLink}": string.Empty;

    /// <summary>
    /// Gets or sets the perma link.
    /// </summary>
    /// <value>The perma link.</value>
    public string PermaLink { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>The title.</value>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the author.
    /// </summary>
    /// <value>The author.</value>
    public string Author { get; set; }

    /// <summary>
    /// Gets or sets the created.
    /// </summary>
    /// <value>The created.</value>
    public DateTime Created { get; set; }

    /// <summary>
    /// Gets or sets up votes.
    /// </summary>
    /// <value>Up votes.</value>
    public int UpVotes { get; set; }

    /// <summary>
    /// Gets or sets down votes.
    /// </summary>
    /// <value>Down votes.</value>
    public int DownVotes { get; set; }
}