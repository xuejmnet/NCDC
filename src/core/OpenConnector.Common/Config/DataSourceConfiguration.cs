using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Text;

namespace OpenConnector.Common.Config
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 14:58:41
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class DataSourceConfiguration
    {
        private static readonly string GETTER_PREFIX = "get";

        private static readonly string SETTER_PREFIX = "set";

        private static readonly ICollection<Type> GENERAL_CLASS_TYPE;

        private static readonly ICollection<string> SKIPPED_PROPERTY_NAMES;

        static DataSourceConfiguration()
        {
            GENERAL_CLASS_TYPE = new HashSet<Type>()
            {
                typeof(bool),
                typeof(int),
                typeof(long),
                typeof(string),
                typeof(ICollection<>),
                typeof(List<>),
            };
            SKIPPED_PROPERTY_NAMES = new HashSet<string>() { "loginTimeout" };
        }

        private readonly DbProviderFactory _dbProviderFactory;
    
    private readonly IDictionary<string, object> _properties = new Dictionary<string, object>();

        public DataSourceConfiguration(DbProviderFactory dbProviderFactory)
        {
            _dbProviderFactory = dbProviderFactory;
        }
        /**
         * Get data source configuration.
         * 
         * @param dataSource data source
         * @return data source configuration
         */
        public static DataSourceConfiguration GetDataSourceConfiguration(DbProviderFactory dbProviderFactory)
        {
            DataSourceConfiguration result = new DataSourceConfiguration(dbProviderFactory);
            return result;
        }



        /**
         * Add alias to share configuration with sharding-jdbc.
         *
         * @param alias alias for configuration
         */
        public void AddAlias(params string[] alias)
        {
            object value = null;
            foreach (var alia in alias)
            {
                if (!_properties.ContainsKey(alia))
                {
                    value = _properties[alia];
                }
            }
            if (null == value)
            {
                return;
            }
            foreach (var alia in alias)
            {
                _properties.Add(alia,value);
            }
        }
    }
}
