using NCDC.CommandParser.Value.Identifier;

namespace NCDC.CommandParser.Segment.Generic.Table
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
        private AliasSegment alias;

        public SimpleTableSegment(int startIndex,int stopIndex,IdentifierValue identifierValue)
        {
            TableName = new TableNameSegment(startIndex, stopIndex, identifierValue);
        }

        public SimpleTableSegment(TableNameSegment tableName)
        {
            _tableName = tableName;
        }
        public TableNameSegment GetTableName()
        {
            return _tableName;
        }
        /// <summary>
        /// 如果不存在所属者就返回表名的开始索引
        /// </summary>
        /// <returns></returns>
        private int GetStartIndex()
        {
            return null == Owner ? TableName.StartIndex : Owner.;
        }

        private int GetStopIndex()
        {
            return _tableName.GetStopIndex();
            //FIXME: Rewriter need to handle alias as well
            //        return null == alias ? tableName.getStopIndex() : alias.getStopIndex();
        }

        public OwnerSegment GetOwner()
        {
            return this.owner;
        }

        public void SetOwner(OwnerSegment owner)
        {
            this.owner = owner;
        }

        public string GetAlias()
        {
            return this.alias.GetIdentifier().GetValue();
        }

        public void SetAlias(AliasSegment alias)
        {
            this.alias = alias;
        }

    }
}
