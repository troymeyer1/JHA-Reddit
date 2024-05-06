using IdentityModel.Client;
using Jha.Reddit.Shared.Entities;
using System.Net.Http.Json;

namespace Jha.Reddit.Business;

/// <summary>
/// Class RedditHttpClient.
/// Implements the <see cref="IRedditHttpClient" />
/// </summary>
/// <param name="tokenHttpClient">The token http client.</param>
/// <param name="configuration">The configuration.</param>
/// <param name="feed">The feed.</param>
/// <seealso cref="IRedditHttpClient" />
public class RedditHttpClient(ITokenHttpClient tokenHttpClient, RedditConfigEntity configuration, RedditFeedEntity feed) : IRedditHttpClient
{
    public const string UserAgent = "JhaRedditAssessment";
    private readonly HttpClient _httpClient = new(new RedditHandler(tokenHttpClient, feed))
    {
        BaseAddress = new(configuration.BaseApiUrl, string.Empty),
        DefaultRequestHeaders =
        {
            { "User-Agent", UserAgent }
        }
    };

    /// <summary>
    /// Sends the asynchronous.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
    {
        return await _httpClient.SendAsync(request);
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        _httpClient?.Dispose();
        GC.SuppressFinalize(this);
    }
}