using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Web;
using Jha.Reddit.Abstractions.Services;
using Jha.Reddit.Business.Messages;
using Jha.Reddit.Shared;
using Jha.Reddit.Shared.Entities;

namespace Jha.Reddit.Business;

/// <summary>
/// Class FeedMonitor.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="FeedMonitor"/> class.
/// </remarks>
/// <param name="httpClient">The http client.</param>
/// <param name="feed">The feed.</param>
public class FeedMonitor(IRedditHttpClient httpClient, RedditFeedEntity feed) : IRedditMonitorService
{
    private readonly IRedditHttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    private readonly RedditFeedStatsEntity _feedStats = new(feed);
    private readonly Dictionary<string, RedditPostEntity> _posts = [];
    private Task _monitorTask;
    private bool _isRunning;

    /// <summary>
    /// Occurs when the top post changed.
    /// </summary>
    public event EventHandler<TopPostChangedEventArgs> TopPostChanged;

    /// <summary>
    /// Occurs when the top user changed.
    /// </summary>
    public event EventHandler<TopUserChangedEventArgs> TopUserChanged;

    /// <summary>
    /// Starts this instance.
    /// </summary>
    public void Start()
    {
        _isRunning = true;
        _monitorTask = Task.Run(Monitor);
    }

    /// <summary>
    /// Stops this instance.
    /// </summary>
    public void Stop()
    {
        _isRunning = false;

        if (_monitorTask == null)
        {
            return;
        }

        _monitorTask.Wait();
        _monitorTask.Dispose();
        _monitorTask = null;
    }

    /// <summary>
    /// Monitors this instance.
    /// </summary>
    /// <returns>System.Threading.Tasks.Task.</returns>
    private async Task Monitor()
    {
        try
        {
            var firstPostPermaLink = string.Empty;

            while (_isRunning)
            {
                try
                {
                    var firstPostFound = false;
                    var after = string.Empty;

                    while (!firstPostFound)
                    {
                        var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{feed.SubReddit}/new.json?limit=100&after={after}");
                        var response = await _httpClient.SendAsync(requestMessage);
                        response.EnsureSuccessStatusCode();

                        var listing = await response.Content.ReadFromJsonAsync<ListingResponse>();
                        var newPosts = listing?.Data?.Children?.Count > 0 ? listing.Data.Children.Select(x => Map(x.Data)).ToList() : [];

                        if (_posts.Count == 0 || newPosts.Any(i => i.PermaLink == firstPostPermaLink))
                        {
                            firstPostPermaLink = newPosts.Last()?.PermaLink;
                            firstPostFound = true;
                        }

                        foreach (var post in newPosts)
                        {
                            _posts.TryAdd(post.PermaLink, post);
                        }

                        var topPost = _posts.Values
                                            .OrderByDescending(i => i.UpVotes)
                                            .ThenBy(i => i.Created)
                                            .FirstOrDefault();

                        var topUser = _posts.Values
                                            .GroupBy(x => x.Author)
                                            .Select(i => new RedditUserStatsEntity()
                                            {
                                                UserName = i.Key,
                                                PostCount = i.Count()
                                            })
                                            .OrderByDescending(i => i.PostCount)
                                            .ThenBy(i => i.UserName)
                                            .FirstOrDefault();

                        var topPostChanged = _feedStats.TopPost?.PermaLink != topPost?.PermaLink;
                        var topUserChanged = _feedStats.TopUser?.UserName != topUser?.UserName;

                        _feedStats.TopPost = topPost;
                        _feedStats.TopUser = topUser;

                        if (topPostChanged)
                        {
                            TopPostChanged?.Invoke(this, new(_feedStats.Feed, _feedStats.TopPost));
                        }

                        if (topUserChanged)
                        {
                            TopUserChanged?.Invoke(this, new(_feedStats.Feed, _feedStats.TopUser));
                        }

                        after = listing.Data.After;

                        if (!response.Headers.TryGetValues(HttpHeaders.RateLimitSleep, out var headerValues))
                        {
                            continue;
                        }

                        var totalSeconds = double.Parse(headerValues.First());
                        var sleepDuration = TimeSpan.FromSeconds(totalSeconds);
                        await Task.Delay(sleepDuration);
                    }
                }
                catch (TaskCanceledException ex)
                {
                    if (ex.InnerException is not TimeoutException)
                    {
                        throw;
                    }

                    Debug.WriteLine($"{DateTime.Now:MM/dd/yyyy hh:mm:ss.fff}: Timeout occurred ...");
                }
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    /// <summary>
    /// Maps the specified post.
    /// </summary>
    /// <param name="post">The post.</param>
    /// <returns>Jha.Reddit.Shared.Entities.RedditPostEntity.</returns>
    private static RedditPostEntity Map(Data post)
    {
        return new()
        {
            PermaLink = post.Permalink,
            Title = HttpUtility.HtmlDecode(post.Title),
            Author = post.Author,
            Created = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(post.CreatedUtc)).DateTime,
            UpVotes = post.Ups,
            DownVotes = post.Downs
        };
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        Stop();
        _httpClient?.Dispose();
        GC.SuppressFinalize(this);
    }
}