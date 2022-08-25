namespace ShardingConnector.CommandParser.Segment.TCL
{
    public sealed class AutoCommitSegment:ISqlSegment
    {
        private readonly int _startIndex;

        private readonly int _stopIndex;

        private readonly bool _autoCommit;

        public AutoCommitSegment(int startIndex, int stopIndex, bool autoCommit)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            _autoCommit = autoCommit;
        }
        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public bool IsAutoCommit()
        {
            return _autoCommit;
        }
    }
}