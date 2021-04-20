using System;

namespace ShardingConnector.AbstractParser.Visitor
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 8:42:03
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    /// 外观模式
    /// </summary>
    public interface ISqlVisitorFacade
    {
        Type GetDMLVisitorType();
        Type GetDDLVisitorType();
        Type GetTCLVisitorType();
        Type GetDCLVisitorType();
        Type GetDALVisitorType();
        Type GetRLVisitorType();
    }
}
