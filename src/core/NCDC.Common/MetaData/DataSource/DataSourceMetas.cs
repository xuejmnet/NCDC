using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCDC.Common.Config;
using OpenConnector.Spi.DataBase.DataBaseType;
using OpenConnector.Spi.DataBase.MetaData;

namespace NCDC.Common.MetaData.DataSource
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 14:50:53
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class DataSourceMetas
    {
        private readonly IDictionary<string, IDataSourceMetaData> _dataSourceMetaDataMap;

        public DataSourceMetas(IDatabaseType databaseType, IDictionary<string, DatabaseAccessConfiguration> databaseAccessConfigurationMap)
        {
            _dataSourceMetaDataMap = GetDataSourceMetaDataMap(databaseType, databaseAccessConfigurationMap);
        }

        private IDictionary<string, IDataSourceMetaData> GetDataSourceMetaDataMap(IDatabaseType databaseType, IDictionary<string, DatabaseAccessConfiguration> databaseAccessConfigurationMap)
        {
            return databaseAccessConfigurationMap.ToDictionary(o=>o.Key,
                o=> databaseType.GetDataSourceMetaData(o.Value.Url));
        }

        /**
         * Get all instance data source names.
         *
         * @return instance data source names
         */
        public ICollection<string> GetAllInstanceDataSourceNames()
        {
            ICollection<string> result = new LinkedList<string>();
            foreach (var dataSourceMetaData in _dataSourceMetaDataMap)
            {
                if (!IsExisted(dataSourceMetaData.Key, result))
                {
                    result.Add(dataSourceMetaData.Key);
                }
            }
            return result;
        }

        private bool IsExisted(string dataSourceName, ICollection<string> existedDataSourceNames)
        {
            return existedDataSourceNames.Any(o=> IsInSameDatabaseInstance(_dataSourceMetaDataMap[dataSourceName],_dataSourceMetaDataMap[o]));
        }
        /// <summary>
        /// 判断是否在自同一个数据库实例里面
        /// </summary>
        /// <param name="sample"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private bool IsInSameDatabaseInstance(IDataSourceMetaData sample, IDataSourceMetaData target)
        {
            return sample is IMemorizedDataSourceMetaData
                ? object.Equals(target.GetSchema(), sample.GetSchema()) : target.GetHostName().Equals(sample.GetHostName()) && target.GetPort() == sample.GetPort();
        }

        /**
         * Get data source meta data.
         * 
         * @param dataSourceName data source name
         * @return data source meta data
         */
        public IDataSourceMetaData GetDataSourceMetaData(string dataSourceName)
        {
            return _dataSourceMetaDataMap[dataSourceName];
        }
    }
}
