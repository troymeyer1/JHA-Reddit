using IdentityModel.Client;
using Jha.Reddit.Shared.Entities;
using System.Net.Http.Json;

namespace Jha.Reddit.Business
{
    /// <summary>
    /// Class TokenHttpClient.
    /// </summary>
    public class TokenHttpClient(RedditConfigEntity configuration) : ITokenHttpClient
    {
        private readonly RedditConfigEntity _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        private readonly HttpClient _httpClient = new()
        {
            BaseAddress = new(configuration.BaseApiUrl, string.Empty),
            DefaultRequestHeaders =
            {
                { "User-Agent", RedditHttpClient.UserAgent }
            }
        };

        /// <summary>
        /// Get the access token as an asynchronous operation.
        /// </summary>
        /// <returns>A Task&lt;AccessTokenEntity&gt; representing the asynchronous operation.</returns>
        public async Task<AccessTokenEntity> GetAccessTokenAsync()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/v1/access_token");
            requestMessage.SetBasicAuthentication(_configuration.ClientId, _configuration.ClientSecret);
            requestMessage.Content = new FormUrlEncodedContent([new("grant_type", "client_credentials")]);

            var response = await _httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
            return response.Content.ReadFromJsonAsync<AccessTokenEntity>().Result;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _httpClient?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
