namespace OpenConnector.CommandParser.Segment.DAL
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 22:25:58
* @Email: 326308290@qq.com
*/
    public sealed class VariableSegment:ISqlSegment
    {
        private readonly int _startIndex;
    
        private readonly int _stopIndex;
    
        private readonly string _variable;

        public VariableSegment(int startIndex, int stopIndex, string variable)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            _variable = variable;
        }

        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public string GetVariable()
        {
            return _variable;
        }
    }
}