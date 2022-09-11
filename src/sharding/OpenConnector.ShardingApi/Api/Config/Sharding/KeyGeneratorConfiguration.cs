using System;
using System.Collections.Generic;
using NCDC.Common.Config;

namespace OpenConnector.ShardingApi.Api.Config.Sharding
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/14 9:23:32
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class KeyGeneratorConfiguration: TypeBasedSPIConfiguration
    {
        public string Column { get; }
        public KeyGeneratorConfiguration(string type,string column) : base(type)
        {
            this.Column = column ?? throw new ArgumentNullException("column");
        }

        public KeyGeneratorConfiguration(string type, string column, IDictionary<string, object> properties) : base(type, properties)
        {
            this.Column = column ?? throw new ArgumentNullException("column");
        }
    }
}
