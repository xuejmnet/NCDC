
using ShardingConnector.CommandParserBinder.MetaData.Schema;

namespace ShardingConnector.RewriteEngine.Sql.Token.Generator.Aware
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 16:16:44
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface ISchemaMetaDataAware
    {
        void SetSchemaMetaData(SchemaMetaData schemaMetaData);
    }
}
