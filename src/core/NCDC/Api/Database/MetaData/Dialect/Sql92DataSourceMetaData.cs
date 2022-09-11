using OpenConnector.Spi.DataBase.MetaData;

namespace OpenConnector.Api.Database.MetaData.Dialect
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/19 11:05:16
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public class Sql92DataSourceMetaData:IDataSourceMetaData
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