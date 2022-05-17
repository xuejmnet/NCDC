using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Spi.DataBase.MetaData;

namespace ShardingConnector.Spi.DataBase.DataBaseType
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 14:53:49
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IDatabaseType
    {
        string GetName();
        /**
         * Get data source meta data.
         * 
         * @param url URL of data source
         * @param username username of data source
         * @return data source meta data
         */
        IDataSourceMetaData GetDataSourceMetaData(string url);
    }
}
