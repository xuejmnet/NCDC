namespace ShardingConnector.CommandParser.Segment.TCL
{
    public sealed class ImplicitTransactionsSegment:ISqlSegment
    {
        private readonly int _startIndex;

        private readonly int _stopIndex;

        private readonly bool _on;

        public ImplicitTransactionsSegment(int startIndex, int stopIndex, bool on)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            _on = on;
        }
        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public bool IsOn()
        {
            return _on;
        }
    }
}