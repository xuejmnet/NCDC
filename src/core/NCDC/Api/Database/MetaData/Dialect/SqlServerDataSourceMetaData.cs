using System.Text.RegularExpressions;
using OpenConnector.Spi.DataBase.MetaData;

namespace OpenConnector.Api.Database.MetaData.Dialect
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/14 12:16:05
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public class SqlServerDataSourceMetaData:IDataSourceMetaData
    {
        private const string SqlServer =
            "adonet:(microsoft:)?sqlserver://([\\w\\-\\.]+):?([0-9]*);\\S*(DatabaseName|database)=([\\w\\-]+);?";
        private static readonly int DEFAULT_PORT = 1433;
    
        private readonly string _hostName;
    
        private readonly int _port;
    
        private readonly string _catalog;
    
        private readonly string _schema;
    
        private readonly Regex _pattern = new Regex(SqlServer,RegexOptions.IgnoreCase);
    
        public SqlServerDataSourceMetaData(string url) {
            //var match = _pattern.Match(url);
            //if (!_pattern.IsMatch(url)) {
            //    throw new ShardingException($"The URL: '{url}' is not recognized. Please refer to this pattern: '{SqlServer}'.");
            //}
            //_hostName = match.Groups[2].Value;
            //_port = string.IsNullOrEmpty(match.Groups[3].Value) ? DEFAULT_PORT : int.Parse(match.Groups[3].Value);
            //_catalog = match.Groups[5].Value;
            //_schema = null;
        }
        public string GetHostName()
        {
            return _hostName;
        }

        public int GetPort()
        {
            return _port;
        }

        public string GetCatalog()
        {
            return _catalog;
        }

        public string GetSchema()
        {
            return _schema;
        }
    }
}