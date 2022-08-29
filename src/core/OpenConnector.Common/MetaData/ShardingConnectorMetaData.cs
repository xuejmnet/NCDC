using System;
using System.Collections.Generic;
using System.Text;
using OpenConnector.CommandParserBinder.MetaData.Schema;
using OpenConnector.Common.MetaData.DataSource;

namespace OpenConnector.Common.MetaData
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 14:50:09
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class OpenConnectorMetaData
    {
        public OpenConnectorMetaData(DataSourceMetas dataSources, SchemaMetaData schema)
        {
            DataSources = dataSources;
            Schema = schema;
        }

        public DataSourceMetas DataSources { get; }

        public SchemaMetaData Schema { get; }
    }
}
