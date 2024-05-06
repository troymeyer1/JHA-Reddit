using Jha.Reddit.Shared.Entities;

namespace Jha.Reddit.Business
{
    /// <summary>
    /// Class HttpClientFactory.
    /// Implements the <see cref="IHttpClientFactory" />
    /// </summary>
    /// <seealso cref="IHttpClientFactory" />
    public class HttpClientFactory : IHttpClientFactory
    {
        /// <summary>
        /// Creates the reddit HTTP client.
        /// </summary>
        /// <param name="tokenHttpClient">The auth token client.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="feed">The feed.</param>
        /// <returns>IRedditHttpClient.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public IRedditHttpClient CreateRedditHttpClient(ITokenHttpClient tokenHttpClient, RedditConfigEntity configuration, RedditFeedEntity feed)
        {
            ArgumentNullException.ThrowIfNull(tokenHttpClient);
            ArgumentNullException.ThrowIfNull(configuration);
            ArgumentNullException.ThrowIfNull(feed);

            return new RedditHttpClient(tokenHttpClient, configuration, feed);
        }

        /// <summary>
        /// Creates the token HTTP client.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>ITokenHttpClient.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public ITokenHttpClient CreateTokenHttpClient(RedditConfigEntity configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);

            return new TokenHttpClient(configuration);
        }
    }
}
