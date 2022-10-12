namespace NCDC.CommandParser.Segment.DAL
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 22:25:58
* @Email: 326308290@qq.com
*/
    public sealed class VariableSegment:ISqlSegment
    {
        public int StartIndex { get; set; }
        public int StopIndex { get; set; }
        public string? Scope { get; set; }
        public string? Variable { get; set; }

        public override string ToString()
        {
            return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(Scope)}: {Scope}, {nameof(Variable)}: {Variable}";
        }
    }
}