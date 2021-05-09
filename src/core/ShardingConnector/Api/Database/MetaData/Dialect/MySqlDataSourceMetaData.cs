using ShardingConnector.Spi.DataBase.MetaData;

namespace ShardingConnector.Api.Database.MetaData.Dialect
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/19 11:08:14
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public class MySqlDataSourceMetaData:IDataSourceMetaData
    {
        public string GetHostName()
        {
            throw new System.NotImplementedException();
        }

        public int GetPort()
        {
            throw new System.NotImplementedException();
        }

        public string GetCatalog()
        {
            throw new System.NotImplementedException();
        }

        public string GetSchema()
        {
            throw new System.NotImplementedException();
        }
    }
}