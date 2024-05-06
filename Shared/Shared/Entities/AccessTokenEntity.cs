using System.Text.Json.Serialization;

namespace Jha.Reddit.Shared.Entities;

/// <summary>
/// Class AccessTokenEntity.
/// </summary>
public class AccessTokenEntity
{
    private const int ExpirationBuffer = 30;
    private readonly DateTime _created;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessTokenEntity"/> class.
    /// </summary>
    public AccessTokenEntity()
    {
        _created = DateTime.Now;
    }

    /// <summary>
    /// Gets or sets the token.
    /// </summary>
    /// <value>The token.</value>
    [JsonPropertyName("access_token")]
    public string Token { get; set; }

    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    /// <value>The type.</value>
    [JsonPropertyName("token_type")]
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the expires in.
    /// </summary>
    /// <value>The expires in.</value>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Gets or sets the scope.
    /// </summary>
    /// <value>The scope.</value>
    [JsonPropertyName("scope")]
    public string Scope { get; set; }

    /// <summary>
    /// Gets a value indicating whether this instance is expired.
    /// </summary>
    /// <value><c>true</c> if this instance is expired; otherwise, <c>false</c>.</value>
    public bool IsExpired => DateTime.Now > _created.AddSeconds(ExpiresIn - ExpirationBuffer);
}