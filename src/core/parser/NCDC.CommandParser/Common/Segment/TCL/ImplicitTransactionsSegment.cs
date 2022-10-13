namespace NCDC.CommandParser.Common.Segment.TCL
{
    public sealed class ImplicitTransactionsSegment:ISqlSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        public bool On { get; }


        public ImplicitTransactionsSegment(int startIndex, int stopIndex, bool on)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            On = on;
        }
    }
}