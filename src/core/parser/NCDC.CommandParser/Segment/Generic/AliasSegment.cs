using NCDC.CommandParser.Value.Identifier;

namespace NCDC.CommandParser.Segment.Generic
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/10 10:07:17
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    /// 数据库别名片断
    /// </summary>
    public sealed class AliasSegment:ISqlSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        public IdentifierValue IdentifierValue { get; }

        public AliasSegment(int startIndex, int stopIndex, IdentifierValue identifierValue)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            IdentifierValue = identifierValue;
        }

        public override string ToString()
        {
            return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(IdentifierValue)}: {IdentifierValue}";
        }
    }
}
