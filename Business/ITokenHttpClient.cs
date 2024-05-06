using Jha.Reddit.Shared.Entities;

namespace Jha.Reddit.Business;

/// <summary>
/// Interface ITokenHttpClient
/// </summary>
public interface ITokenHttpClient : IDisposable
{
    /// <summary>
    /// Get the access token as an asynchronous operation.
    /// </summary>
    /// <returns>A Task&lt;AccessTokenEntity&gt; representing the asynchronous operation.</returns>
    Task<AccessTokenEntity> GetAccessTokenAsync();
}