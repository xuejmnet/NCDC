using System.Data.Common;

namespace OpenConnector.Spi.DataBase.DataBaseType.DataBaseDiscover
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/25 17:09:08
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IDataBaseTypeDiscover
    {
        bool Match(DbConnection connection);
        string DataBaseTypeName { get; }
    }
}
