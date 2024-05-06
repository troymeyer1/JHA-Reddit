namespace Jha.Reddit.Business;

/// <summary>
/// Interface IRedditHttpClient
/// </summary>
public interface IRedditHttpClient : IDisposable
{
    /// <summary>
    /// Sends the asynchronous.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
}