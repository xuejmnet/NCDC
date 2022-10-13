using NCDC.CommandParser.Common.Segment.Generic;

namespace NCDC.CommandParser.Common.Segment.DML.Item
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 21:55:09
* @Email: 326308290@qq.com
*/
    public sealed class ShorthandProjectionSegment:IProjectionSegment,IOwnerAvailable,IAliasAvailable
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
    
        public OwnerSegment? Owner { get; set; }
        private AliasSegment? _alias;

        public ShorthandProjectionSegment(int startIndex, int stopIndex)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
        }

        public string? GetAlias()
        {
            return _alias?.IdentifierValue.Value;
        }

        public void SetAlias(AliasSegment alias)
        {
            this._alias = alias;
        }

        public override string ToString()
        {
            return $"Alias: {GetAlias()}, {nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(Owner)}: {Owner}";
        }
    }
}