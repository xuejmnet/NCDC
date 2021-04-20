namespace ShardingConnector.ParserBinder.MetaData.Index
{
/*
* @Author: xjm
* @Description:
* @Date: Thursday, 08 April 2021 22:08:49
* @Email: 326308290@qq.com
*/
    public sealed class IndexMetaData
    {
        public IndexMetaData(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}