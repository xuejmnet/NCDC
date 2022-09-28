using System.Collections.Concurrent;
using NCDC.Enums;
using NCDC.Exceptions;
using NCDC.ProxyServer.Configurations;
using NCDC.ProxyServer.Connection.User;

namespace NCDC.ProxyServer.Options;


    /// <summary>
    /// 分片配置
    /// </summary>
    public class ShardingConfigOption
    {
        public string DatabaseName { get; }
        /// <summary>
        /// 写操作数据库后自动使用写库链接防止读库链接未同步无法查询到数据
        /// </summary>
        public bool AutoUseWriteConnectionStringAfterWriteDb { get; set; } = false;
        /// <summary>
        /// 当查询遇到没有路由被命中时是否抛出错误
        /// </summary>
        public bool ThrowIfQueryRouteNotMatch { get; set; } = true;

        /// <summary>
        /// 全局配置最大的查询连接数限制,默认系统逻辑处理器<code>Environment.ProcessorCount</code>
        /// </summary>
        public int MaxQueryConnectionsLimit { get; set; } = Environment.ProcessorCount;
        /// <summary>
        /// 默认<code>ConnectionModeEnum.SYSTEM_AUTO</code>
        /// </summary>
        public ConnectionModeEnum ConnectionMode { get; set; } = ConnectionModeEnum.SYSTEM_AUTO;

        /// <summary>
        /// 默认数据源
        /// </summary>
        public string DefaultDataSourceName { get;  set; }
        /// <summary>
        /// 默认数据源链接字符串
        /// </summary>
        public string DefaultConnectionString { get;  set; }

        public IDictionary<string, string> DataSources { get; }
        public IDictionary<string, Grantee> Users { get; }
        /// <summary>
        /// 添加默认数据源
        /// </summary>
        /// <param name="dataSourceName"></param>
        /// <param name="connectionString"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddDefaultDataSource(string dataSourceName, string connectionString)
        {
            DefaultDataSourceName= dataSourceName?? throw new ArgumentNullException(nameof(dataSourceName));
            DefaultConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            if (DataSources.ContainsKey(dataSourceName))
            {
                throw new ShardingConfigException($"default datasource repeat:[{dataSourceName}]");
            }

            DataSources.TryAdd(dataSourceName, connectionString);
        }
        /// <summary>
        /// 添加额外数据源
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddExtraDataSource(string dataSourceName, string connectionString)
        { 
            if (DataSources.ContainsKey(dataSourceName))
            {
                throw new ShardingConfigException($"extra datasource repeat:[{dataSourceName}]");
            }

            DataSources.TryAdd(dataSourceName, connectionString);
        }

        public void AddUser(Grantee user)
        {
            if (Users.ContainsKey(user.Username))
            {
                throw new ShardingConfigException($"auth user repeat:[{user.Username}]");
            }

            Users.TryAdd(user.Username, user);
        }

        public ShardingConfigOption(string databaseName)
        {
            DatabaseName = databaseName;
            DataSources = new ConcurrentDictionary<string, string>();
            Users = new ConcurrentDictionary<string, Grantee>();
        }
        public void CheckArguments()
        {
            if (string.IsNullOrWhiteSpace(DefaultDataSourceName))
                throw new ArgumentNullException(
                    $"{nameof(DefaultDataSourceName)} plz call {nameof(AddDefaultDataSource)}");
            
            if (string.IsNullOrWhiteSpace(DefaultConnectionString))
                throw new ArgumentNullException(
                    $"{nameof(DefaultConnectionString)} plz call {nameof(AddDefaultDataSource)}");

            if (MaxQueryConnectionsLimit <= 0)
                throw new ArgumentException(
                    $"{nameof(MaxQueryConnectionsLimit)} should greater than and equal 1");
        }

    }