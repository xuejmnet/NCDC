using NCDC.Exceptions;

namespace NCDC.Base;

public class OneByOneChecker
{
    private readonly OneByOneCheck _oneByOne;

    public OneByOneChecker()
    {
        _oneByOne = new OneByOneCheck();
    }

    public void OneByOneLock()
    {
        //是否触发并发了
        var acquired = _oneByOne.Start();
        if (!acquired)
        {
            throw new ShardingException($"{nameof(OneByOneLock)} cant parallel use in connection ");
        }
    }

    public void OneByOneUnLock()
    {
        _oneByOne.Stop();
    }
}