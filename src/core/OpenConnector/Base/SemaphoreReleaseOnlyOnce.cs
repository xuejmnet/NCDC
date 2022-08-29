namespace OpenConnector.Base;

public sealed class SemaphoreReleaseOnlyOnce
{
    private readonly DoOnlyOnce _doOnlyOnce = new DoOnlyOnce();
    private readonly SemaphoreSlim _semaphore;

    public SemaphoreReleaseOnlyOnce(SemaphoreSlim semaphore)
    {
        _semaphore = semaphore;
    }

    public void Release()
    {
        if (_doOnlyOnce.IsUnDo())
        {
            _semaphore.Release();
        }
    }
}