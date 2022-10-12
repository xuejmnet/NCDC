using NCDC.CommandParser.Segment.DML.Column;
using NCDC.CommandParser.Segment.Generic;

namespace NCDC.CommandParser.Segment.DML.Item
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 20:17:02
* @Email: 326308290@qq.com
*/
    public sealed class ColumnProjectionSegment:IProjectionSegment, IAliasAvailable
    {
        public ColumnSegment Column { get; }

        private AliasSegment? _alias;

        public ColumnProjectionSegment(ColumnSegment column)
        {
            Column = column;
        }

        public string? GetAlias()
        {
            return _alias?.IdentifierValue.Value;
        }
      

        public void SetAlias(AliasSegment alias)
        {
            this._alias = alias;
        }

        public int StartIndex => Column.StartIndex;
        public int StopIndex => _alias?.StopIndex ?? Column.StopIndex;

        public override string ToString()
        {
            return $"Alias: {_alias}, {nameof(Column)}: {Column}, {nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}";
        }
    }
}