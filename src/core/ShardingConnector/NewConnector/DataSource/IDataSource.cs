using System.Data.Common;

namespace ShardingConnector.NewConnector.DataSource
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/19 13:51:22
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IDataSource
    {
        DbConnection CreateConnection();
    }
}
