namespace NCDC.Base;

public sealed class DoOnlyOnce
{
    private const int Did = 1;
    private const int UnDo = 0;
    private int Status = UnDo;

    public bool IsUnDo()
    {
        return Interlocked.CompareExchange(ref Status, Did, UnDo) == UnDo;
    }

    public void Reset()
    {
        Interlocked.Exchange(ref Status, UnDo);
    }
}

