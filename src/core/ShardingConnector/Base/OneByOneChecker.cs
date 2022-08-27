using System.Threading;
using ShardingConnector.Exceptions;

namespace ShardingConnector.Base;

public class OneByOneChecker
{
    /// <summary>
    /// running const mark
    /// </summary>
    private const int running = 1;

    /// <summary>
    /// not running const mark
    /// </summary>
    private const int unrunning = 0;
    /// <summary>
    /// run status
    /// </summary>
    private int runStatus;

    public OneByOneChecker()
    {
        runStatus = unrunning;
    }
    public bool Start()
    {
        return Interlocked.CompareExchange(ref runStatus, running, unrunning) == unrunning;
    }

    public bool IsRunning()
    {
        return runStatus == running;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mustExchange">must exchange</param>
    public void Stop(bool mustExchange=false)
    {
        if (Interlocked.Exchange(ref runStatus, unrunning) != running&& !mustExchange)
        {
            throw new ShardingException("one by one stop error,current is not running");
        }
    }
}