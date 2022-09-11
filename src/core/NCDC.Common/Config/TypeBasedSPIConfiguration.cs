using System;
using System.Collections.Generic;
using System.Text;

namespace NCDC.Common.Config
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/14 9:23:47
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public abstract class TypeBasedSPIConfiguration
    {
        public  string @Type { get; }

        public IDictionary<string,object> Properties { get; }

        protected TypeBasedSPIConfiguration(string type):this(type, null)
        {
            
        }

        protected TypeBasedSPIConfiguration(string type, IDictionary<string, object> properties)
        {
            this.@Type = type??throw new ArgumentNullException("Type is required.");
            this.Properties = properties??new Dictionary<string, object>();
        }
    }
}
