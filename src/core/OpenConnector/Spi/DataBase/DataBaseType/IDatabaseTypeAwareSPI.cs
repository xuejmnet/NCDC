using System;
using System.Collections.Generic;
using System.Text;

namespace OpenConnector.Spi.DataBase.DataBaseType
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 14:55:33
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IDatabaseTypeAwareSPI
    {
        string GetDatabaseType();
    }
}
