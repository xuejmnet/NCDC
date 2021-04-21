using System;
using ShardingConnector.AbstractParser.Visitor;
using ShardingConnector.SqlServerParser.Visitor.Impl;

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
            return typeof(SqlServerDMLVisitor);
        }

        public Type GetDDLVisitorType()
        {
            return typeof(SqlServerDDLVisitor);
        }

        public Type GetTCLVisitorType()
        {
            return typeof(SqlServerTCLVisitor);
        }

        public Type GetDCLVisitorType()
        {
            return typeof(SqlServerDCLVisitor);
        }

        public Type GetDALVisitorType()
        {
            return typeof(SqlServerDALVisitor);
        }

        public Type GetRLVisitorType()
        {
            return null;
        }
    }
}