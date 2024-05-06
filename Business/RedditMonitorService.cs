using Jha.Reddit.Abstractions.Services;
using Jha.Reddit.Shared;
using Jha.Reddit.Shared.Entities;

namespace Jha.Reddit.Business;

/// <summary>
/// Class RedditMonitorService.
/// Implements the <see cref="IRedditMonitorService" />
/// </summary>
/// <seealso cref="IRedditMonitorService" />
/// <remarks>
/// Initializes a new instance of the <see cref="RedditMonitorService"/> class.
/// </remarks>
/// <param name="configuration">The configuration.</param>
/// <exception cref="ArgumentNullException">configuration</exception>
public class RedditMonitorService(RedditConfigEntity configuration) : IRedditMonitorService
{
    private readonly RedditConfigEntity _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    private readonly List<IRedditMonitorService> FeedMonitors = [];
    private bool _running;

    /// <summary>
    /// Occurs when the top post has changed.
    /// </summary>
    public event EventHandler<TopPostChangedEventArgs> TopPostChanged;

    /// <summary>
    /// Occurs when the top user has changed.
    /// </summary>
    public event EventHandler<TopUserChangedEventArgs> TopUserChanged;

    /// <summary>
    /// Starts this instance.
    /// </summary>
    public void Start()
    {
        if (_running)
            return;

        _running = true;
        foreach (var feed in _configuration.Feeds)
        {
            var monitor = new FeedMonitor(configuration, feed);
            monitor.TopPostChanged += OnTopPostChanged;
            monitor.TopUserChanged += OnTopUserChanged;
            FeedMonitors.Add(monitor);
            monitor.Start();
        }
    }

    /// <summary>
    /// Handles the <see cref="E:TopUserChanged" /> event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="TopUserChangedEventArgs"/> instance containing the event data.</param>
    private void OnTopUserChanged(object sender, TopUserChangedEventArgs e)
    {
        TopUserChanged?.Invoke(this, e);
    }

    /// <summary>
    /// Handles the <see cref="E:TopPostChanged" /> event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="TopPostChangedEventArgs"/> instance containing the event data.</param>
    private void OnTopPostChanged(object sender, TopPostChangedEventArgs e)
    {
        TopPostChanged?.Invoke(this, e);
    }

    /// <summary>
    /// Stops this instance.
    /// </summary>
    public void Stop()
    {
        _running = false;
        foreach (var monitor in FeedMonitors)
        {
            monitor.Stop();
            monitor.TopPostChanged -= OnTopPostChanged;
            monitor.TopUserChanged -= OnTopUserChanged;
        }
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        foreach (var monitor in FeedMonitors)
        {
            monitor.Dispose();
        }

        GC.SuppressFinalize(this);
    }
}