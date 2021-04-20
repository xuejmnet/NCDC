using ShardingConnector.CommandParser.Segment.Generic.Table;

namespace ShardingConnector.CommandParser.Segment
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 13:28:41
* @Email: 326308290@qq.com
*/
    public sealed class JoinedTableSegment:ISqlSegment
    {
        
        private int _startIndex;
    
        private int _stopIndex;
    
        private TableFactorSegment _tableFactor;
    
        private JoinSpecificationSegment _joinSpecification;
        
        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public TableFactorSegment GetTableFactor()
        {
            return this._tableFactor;
        }
        public void SetTableFactor(TableFactorSegment tableFactor)
        {
            this._tableFactor = tableFactor;
        }

        public JoinSpecificationSegment GetJoinSpecification()
        {
            return _joinSpecification;
        }

        public void SetJoinSpecification(JoinSpecificationSegment joinSpecification)
        {
            this._joinSpecification = joinSpecification;
        }
        
        /**
     * get table.
     * @return tableSegment.
     */
        public ITableSegment GetTable() {
            if (null != _tableFactor.GetTable()) {
                if (_tableFactor.GetTable() is SimpleTableSegment simpleTable) {
                    return simpleTable;
                }
            }
            return null;
        }
    }
}