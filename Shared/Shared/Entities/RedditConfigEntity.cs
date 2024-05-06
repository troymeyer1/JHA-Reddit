using System.ComponentModel.DataAnnotations;

namespace Jha.Reddit.Shared.Entities;

public class RedditConfigEntity : IValidatableObject
{
    /// <summary>
    /// Gets or sets the base token URL.
    /// </summary>
    /// <value>The base token URL.</value>
    [Required]
    public Uri BaseTokenUrl { get; set; }

    /// <summary>
    /// Gets or sets the base API URL.
    /// </summary>
    /// <value>The base API URL.</value>
    [Required]
    public Uri BaseApiUrl { get; set; }

    /// <summary>
    /// Gets or sets the client identifier.
    /// </summary>
    /// <value>The client identifier.</value>
    [Required]
    public string ClientId { get; set; }

    /// <summary>
    /// Gets or sets the client secret.
    /// </summary>
    /// <value>The client secret.</value>
    [Required]
    public string ClientSecret { get; set; }

    /// <summary>
    /// Gets or sets the feeds.
    /// </summary>
    /// <value>The feeds.</value>
    [Required]
    public List<RedditFeedEntity> Feeds { get; set; }

    /// <summary>Determines whether the specified object is valid.</summary>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>A collection that holds failed-validation information.</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Feeds is { Count: 0 })
        {
            yield return new("No feeds found in configuration file.", [nameof(Feeds)]);
        }
        else
        {
            for (var i = 0; i < Feeds.Count; i++)
            {
                var feed = Feeds[i];
                var feedResults = new List<ValidationResult>();
                
                if (Validator.TryValidateObject(feed, new(feed), feedResults, true))
                {
                    continue;
                }

                foreach (var validationResult in feedResults)
                {
                    var index = i;
                    yield return new($"Feeds[{index}]: {validationResult.ErrorMessage}", validationResult.MemberNames.Select(x => $"Feeds[{index}].{x}"));
                }
            }
        }
    }
}