using Nito.AsyncEx;

namespace NCDC.Configuration.Session;

public sealed class ChannelIsWritableListener
{
    private readonly AsyncManualResetEvent _asyncManualResetEvent;

    public ChannelIsWritableListener()
    {
        //默认设置为wait阻塞
        _asyncManualResetEvent = new AsyncManualResetEvent(false);
    }

    public Task<bool> WaitAsync(TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        return WaitAsync0(timeout, cancellationToken);
    }

    /// <summary>
    /// 等待超时如果超时那么就设置为默认模式
    /// </summary>
    /// <param name="timeout"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>true表示未超时返回,false表示超时返回</returns>
    private async Task<bool> WaitAsync0(TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        var isTimeout = false;
        var eventTask = _asyncManualResetEvent.WaitAsync(cancellationToken);
        if (await Task.WhenAny(eventTask, Task.Delay(timeout, cancellationToken)) != eventTask)
        {
            isTimeout = true;
            //超时了先取消掉之前的wait的task
            _asyncManualResetEvent.Set();
        }
        //将AsyncManualResetEvent设置为wait等待模式
        _asyncManualResetEvent.Reset();
        return !isTimeout;
    }

    /// <summary>
    /// 唤醒
    /// </summary>
    public void Wakeup()
    {
        _asyncManualResetEvent.Set();
    }
}