using OpenConnector.CommandParser.Segment.DML.Column;
using OpenConnector.CommandParser.Segment.Generic;

namespace OpenConnector.CommandParser.Segment.DML.Item
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 20:17:02
* @Email: 326308290@qq.com
*/
    public sealed class ColumnProjectionSegment:IProjectionSegment, IAliasAvailable
    {
        private readonly ColumnSegment _column;
    
        private AliasSegment _alias;

        public ColumnProjectionSegment(ColumnSegment column) {
            _column = column;
        }

        public int GetStartIndex()
        {
            return _column.GetStartIndex();
        }

        public int GetStopIndex()
        {
            return _column.GetStopIndex();
        }

        public string GetAlias()
        {
            return _alias?.GetIdentifier().GetValue();
        }
        public ColumnSegment GetColumn()
        {
            return _column;
        }

        public void SetAlias(AliasSegment alias)
        {
            this._alias = alias;
        }
    }
}