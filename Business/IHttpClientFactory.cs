using Jha.Reddit.Shared.Entities;

namespace Jha.Reddit.Business
{
    /// <summary>
    /// Interface IHttpClientFactory
    /// </summary>
    public interface IHttpClientFactory
    {
        /// <summary>
        /// Creates the reddit HTTP client.
        /// </summary>
        /// <param name="tokenHttpClient">The auth token client.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="feed">The feed.</param>
        /// <returns>IRedditHttpClient.</returns>
        IRedditHttpClient CreateRedditHttpClient(ITokenHttpClient tokenHttpClient, RedditConfigEntity configuration, RedditFeedEntity feed);

        /// <summary>
        /// Creates the token HTTP client.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>ITokenHttpClient.</returns>
        ITokenHttpClient CreateTokenHttpClient(RedditConfigEntity configuration);
    }
}
