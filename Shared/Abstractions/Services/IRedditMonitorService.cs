using Jha.Reddit.Shared;

namespace Jha.Reddit.Abstractions.Services;

/// <summary>
/// Interface IRedditMonitorService
/// </summary>
public interface IRedditMonitorService : IDisposable
{
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
    void Start();

    /// <summary>
    /// Stops this instance.
    /// </summary>
    void Stop();
}