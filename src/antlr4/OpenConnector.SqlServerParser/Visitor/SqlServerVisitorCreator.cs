using System;
using OpenConnector.CommandParser.Abstractions.Visitor;
using OpenConnector.CommandParser.Abstractions.Visitor.Commands;
using OpenConnector.SqlServerParser.Visitor.Impl;

namespace OpenConnector.SqlServerParser.Visitor
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 21:18:19
* @Email: 326308290@qq.com
*/
    public sealed class SqlServerVisitorCreator:ISqlVisitorCreator
    {
        //public Type GetDMLVisitorType()
        //{
        //    return typeof(SqlServerDMLVisitor);
        //}

        //public Type GetDDLVisitorType()
        //{
        //    return typeof(SqlServerDDLVisitor);
        //}

        //public Type GetTCLVisitorType()
        //{
        //    return typeof(SqlServerTCLVisitor);
        //}

        //public Type GetDCLVisitorType()
        //{
        //    return typeof(SqlServerDCLVisitor);
        //}

        //public Type GetDALVisitorType()
        //{
        //    return typeof(SqlServerDALVisitor);
        //}

        //public Type GetRLVisitorType()
        //{
        //    return null;
        //}

        public IDMLVisitor CreateDMLVisitor()
        {
            return new SqlServerDMLVisitor();
        }

        public IDDLVisitor CreateDDLVisitor()
        {
            return new SqlServerDDLVisitor();
        }

        public ITCLVisitor CreateTCLVisitor()
        {
            return new SqlServerTCLVisitor();
        }

        public IDCLVisitor CreateDCLVisitor()
        {
            return new SqlServerDCLVisitor();
        }

        public IDALVisitor CreateDALVisitor()
        {
            return new SqlServerDALVisitor();
        }

        public IRLVisitor CreateRLVisitor()
        {
            throw new NotImplementedException();
        }
    }
}