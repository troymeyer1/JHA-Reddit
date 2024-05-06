using IdentityModel.Client;
using Jha.Reddit.Shared.Entities;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Threading.RateLimiting;

namespace Jha.Reddit.Business;

/// <summary>
/// Class RedditRateLimitedHandler. This class cannot be inherited.
/// Implements the <see cref="DelegatingHandler" />
/// </summary>
/// <param name="tokenHttpClient">The token http client.</param>
/// <param name="feed">The feed.</param>
/// <seealso cref="DelegatingHandler" />
internal sealed class RedditHandler(ITokenHttpClient tokenHttpClient, RedditFeedEntity feed) : DelegatingHandler(new HttpClientHandler())
{
    private const double MinSleepSeconds = 0.1;

    private readonly ITokenHttpClient _tokenHttpClient = tokenHttpClient ?? throw new ArgumentNullException(nameof(tokenHttpClient));
    private readonly RedditFeedEntity _feed = feed ?? throw new ArgumentNullException(nameof(feed));
    private TokenBucketRateLimiter _rateLimiter;
    private RedditRateSettings _initialRateSettings;
    private RedditRateSettings _currentRateSettings;
    private AccessTokenEntity _accessToken;

    /// <summary>
    /// Send as an asynchronous operation.
    /// </summary>
    /// <param name="request">The HTTP request message to send to the server.</param>
    /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
    /// <returns>A Task&lt;HttpResponseMessage&gt; representing the asynchronous operation.</returns>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpResponseMessage response;

        lock (this)
        {
            if (_accessToken == null || _accessToken.IsExpired)
            {
                _accessToken = _tokenHttpClient.GetAccessTokenAsync().Result; //run sync because we're inside a lock
            }

            if (_rateLimiter == null)
            {
                WriteDebug($"Sending Request: {request.RequestUri}");
                response = Send(request, cancellationToken);
                response.EnsureSuccessStatusCode();

                _currentRateSettings = GetRateSettings(response);
                WriteDebug(_currentRateSettings.ToString());

                _rateLimiter ??= GetRateLimiter(_currentRateSettings);
                _initialRateSettings = _currentRateSettings;
                return response;
            } 
            
            if (_currentRateSettings?.RateLimitReset > _initialRateSettings?.RateLimitReset || _currentRateSettings?.RateLimitRemaining > _initialRateSettings?.RateLimitRemaining)
            {
                _rateLimiter = GetRateLimiter(_currentRateSettings);
                _initialRateSettings = _currentRateSettings;
            }
        }

        using var lease = await _rateLimiter.AcquireAsync(1, cancellationToken);
        
        if (lease.IsAcquired)
        {
            WriteDebug($"Sending Request: {request.RequestUri}");
            request.SetBearerToken(_accessToken.Token);

            response = await base.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
            
            _currentRateSettings = GetRateSettings(response);
            WriteDebug(_currentRateSettings.ToString());

            if (_currentRateSettings.RateLimitRemaining > _currentRateSettings.RateLimitReset)
            {
                return response;
            }

            var totalSeconds = (double)_currentRateSettings.RateLimitReset / _currentRateSettings.RateLimitRemaining;
            var sleepDuration = TimeSpan.FromSeconds(totalSeconds);

            if (sleepDuration.TotalSeconds < MinSleepSeconds)
            {
                sleepDuration = TimeSpan.FromSeconds(MinSleepSeconds);
            }

            response.Headers.Add(HttpHeaders.RateLimitSleep, sleepDuration.TotalSeconds.ToString(CultureInfo.InvariantCulture));
            WriteDebug($"Sleeping for {sleepDuration}");
        }
        else
        {
            response = new(HttpStatusCode.TooManyRequests)
            {
                Content = new StringContent("Rate limit exceeded.")
            };
        }

        return response;

    }

    /// <summary>
    /// Writes the debug message.
    /// </summary>
    /// <param name="message">The message.</param>
    private void WriteDebug(string message)
    {
        Debug.WriteLine($"{DateTime.Now:MM/dd/yyyy hh:mm:ss.fff}: Feed: {_feed.Name}, {message} ...");
    }

    /// <summary>
    /// Gets the rate limiter.
    /// </summary>
    /// <param name="rateSettings">The rate settings.</param>
    /// <returns>TokenBucketRateLimiter.</returns>
    private TokenBucketRateLimiter GetRateLimiter(RedditRateSettings rateSettings)
    {
        var tokenCount = rateSettings.RateLimitRemaining - 1;

        WriteDebug($"Initializing Rate Limiter, Token Limit: {tokenCount}, ReplenishmentPeriod: {rateSettings.RateLimitReset} seconds ...");

        return new(new()
        {
            TokenLimit = tokenCount,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = tokenCount,
            ReplenishmentPeriod = TimeSpan.FromSeconds(rateSettings.RateLimitReset),
            TokensPerPeriod = tokenCount,
            AutoReplenishment = true
        });
    }

    /// <summary>
    /// Gets the rate settings.
    /// </summary>
    /// <param name="response">The response.</param>
    /// <returns>Jha.Reddit.Business.RedditRateSettings.</returns>
    private static RedditRateSettings GetRateSettings(HttpResponseMessage response)
    {
        var rateLimitUsed = ParseHeaderValue(response, HttpHeaders.RateLimitUsed);
        var rateLimitRemaining = ParseHeaderValue(response, HttpHeaders.RateLimitRemaining);
        var rateLimitReset = ParseHeaderValue(response, HttpHeaders.RateLimitReset);
        return new(rateLimitUsed, rateLimitRemaining, rateLimitReset);
    }

    /// <summary>
    /// Parses the header value.
    /// </summary>
    /// <param name="response">The response.</param>
    /// <param name="headerKey">The header key.</param>
    /// <returns>int.</returns>
    /// <exception cref="InvalidOperationException">$"Failed to retrieve the header '{headerKey}': Header not found.</exception>
    /// <exception cref="InvalidOperationException">$"Failed to retrieve the header '{headerKey}': Expected 1 value found {headerValues.Count}.</exception>
    /// <exception cref="InvalidOperationException">$"Failed to retrieve the header '{headerKey}': Expected an integer value found '{headerValue}'.</exception>
    private static int ParseHeaderValue(HttpResponseMessage response, string headerKey)
    {
        if (!response.Headers.TryGetValues(headerKey, out var stringValues))
        {
            throw new InvalidOperationException($"Failed to retrieve the header '{headerKey}': Header not found.");
        }

        var headerValues = stringValues.ToList();
        if (headerValues.Count != 1)
        {
            throw new InvalidOperationException($"Failed to retrieve the header '{headerKey}': Expected 1 value found {headerValues.Count}.");
        }

        var headerValue = headerValues.First();
        if (!decimal.TryParse(headerValue, out var decimalValue))
        {
            throw new InvalidOperationException($"Failed to retrieve the header '{headerKey}': Expected a numeric value found '{headerValue}'.");
        }

        return Convert.ToInt32(decimalValue);
    }
}