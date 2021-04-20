using System;
using ShardingConnector.AbstractParser.Visitor;

namespace ShardingConnector.SqlServerParser.Visitor
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 21:18:19
* @Email: 326308290@qq.com
*/
    public sealed class SqlServerVisitorFacade:ISqlVisitorFacade
    {
        public Type GetDMLVisitorType()
        {
            return typeof(sqlserverdml)
        }

        public Type GetDDLVisitorType()
        {
            throw new NotImplementedException();
        }

        public Type GetTCLVisitorType()
        {
            throw new NotImplementedException();
        }

        public Type GetDCLVisitorType()
        {
            throw new NotImplementedException();
        }

        public Type GetDALVisitorType()
        {
            throw new NotImplementedException();
        }

        public Type GetRLVisitorType()
        {
            throw new NotImplementedException();
        }
    }
}