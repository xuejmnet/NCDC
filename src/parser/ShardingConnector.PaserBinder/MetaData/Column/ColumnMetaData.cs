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
        public ColumnMetaData(string name, int dataType, string dataTypeName, bool primaryKey, bool generated, bool caseSensitive)
        {
            Name = name;
            DataType = dataType;
            DataTypeName = dataTypeName;
            PrimaryKey = primaryKey;
            Generated = generated;
            CaseSensitive = caseSensitive;
        }

        public  string Name { get; }
    
        public int DataType{ get; }
    
        public string DataTypeName{ get; }
    
        public bool PrimaryKey{ get; }
    
        public bool Generated{ get; }
    
        public bool CaseSensitive{ get; }
    }
}