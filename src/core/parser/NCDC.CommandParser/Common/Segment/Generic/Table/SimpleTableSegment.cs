using NCDC.CommandParser.Common.Value.Identifier;

namespace NCDC.CommandParser.Common.Segment.Generic.Table
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/10 13:23:15
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SimpleTableSegment:ITableSegment,IOwnerAvailable
    {
        public int StartIndex => GetStartIndex();
        public int StopIndex => GetStopIndex();
        public OwnerSegment? Owner { get; set; }
        
        public TableNameSegment TableName { get; }
        
        private AliasSegment? _alias;

        public SimpleTableSegment(TableNameSegment tableName)
        {
            TableName = tableName;
        }
        /// <summary>
        /// 如果不存在所属者就返回表名的开始索引
        /// </summary>
        /// <returns></returns>
        private int GetStartIndex()
        {
            return Owner?.StartIndex?? TableName.StartIndex;
        }

        private int GetStopIndex()
        {
            return _alias?.StopIndex ?? TableName.StopIndex;
        }

        public string? GetAlias()
        {
            return this._alias?.IdentifierValue.Value;
        }

        public void SetAlias(AliasSegment? alias)
        {
            this._alias = alias;
        }

    }
}
