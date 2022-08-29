using Nito.AsyncEx;

namespace OpenConnector.ProxyServer.Session;

public sealed class ChannelIsWritableListener
{
    private readonly AsyncManualResetEvent _asyncManualResetEvent;

    public ChannelIsWritableListener()
    {
        _asyncManualResetEvent = new AsyncManualResetEvent(false);
    }

    public Task<bool> WaitAsync(TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        //将AsyncManualResetEvent设置为wait等待模式
        _asyncManualResetEvent.Reset();
        return WaitAutoSetIfTimeoutAsync(timeout, cancellationToken);
    }

    /// <summary>
    /// 等待超时如果超时那么就设置为默认模式
    /// </summary>
    /// <param name="timeout"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task<bool> WaitAutoSetIfTimeoutAsync(TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        
        var eventTask = _asyncManualResetEvent.WaitAsync(cancellationToken);
        if (await Task.WhenAny(eventTask, Task.Delay(timeout, cancellationToken)) == eventTask)
        {
            return true;
        }
        _asyncManualResetEvent.Set();
        return false;
    }

    public void Set()
    {
        _asyncManualResetEvent.Set();
    }
}