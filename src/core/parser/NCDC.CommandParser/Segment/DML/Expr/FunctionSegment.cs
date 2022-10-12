using NCDC.CommandParser.Segment.DML.Expr.Complex;
using NCDC.CommandParser.Segment.Generic;

namespace NCDC.CommandParser.Segment.DML.Expr;

public sealed class FunctionSegment:IComplexExpressionSegment
{
    public FunctionSegment(int startIndex, int stopIndex,string functionName, string text)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
        FunctionName = functionName;
        Text = text;
        Parameters = new LinkedList<IExpressionSegment>();
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    public string FunctionName { get; }
    public string Text { get; }
    public ICollection<IExpressionSegment> Parameters { get; }
    public OwnerSegment? Owner { get; set; }
}