namespace ShardingConnector.ParserBinder.MetaData.Column
{
/*
* @Author: xjm
* @Description:
* @Date: Thursday, 08 April 2021 22:06:17
* @Email: 326308290@qq.com
*/
    public class ColumnMetaData
    {
        public ColumnMetaData(string name,int columnOrdinal, string dataTypeName, bool primaryKey, bool generated, bool caseSensitive)
        {
            Name = name;
            ColumnOrdinal = columnOrdinal;
            DataTypeName = dataTypeName;
            PrimaryKey = primaryKey;
            Generated = generated;
            CaseSensitive = caseSensitive;
        }

        public  string Name { get; }
        public int ColumnOrdinal { get; }

        public string DataTypeName{ get; }
    
        public bool PrimaryKey{ get; }
    
        public bool Generated{ get; }
    
        public bool CaseSensitive{ get; }
    }
}