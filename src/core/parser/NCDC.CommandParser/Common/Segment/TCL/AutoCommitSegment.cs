namespace NCDC.CommandParser.Common.Segment.TCL
{
    public sealed class AutoCommitSegment:ISqlSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        public bool AutoCommit { get; }


        public AutoCommitSegment(int startIndex, int stopIndex, bool autoCommit)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            AutoCommit = autoCommit;
        }
    }
}