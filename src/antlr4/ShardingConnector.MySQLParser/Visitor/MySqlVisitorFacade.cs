using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.AbstractParser.Visitor;
using ShardingConnector.AbstractParser.Visitor.Commands;
using ShardingConnector.MySQLParser.Visitor.Impl;

namespace ShardingConnector.MySQLParser.Visitor
{
    /// <summary>
    /// 
    /// </summary>
    /// Author: xjm
    /// Created: 2022/5/10 9:04:01
    /// Email: 326308290@qq.com
    public sealed class MySQLVisitorFacade: ISqlVisitorFacade
    {
        //public Type GetDMLVisitorType()
        //{
        //    return typeof(MySQLDMLVisitor);
        //}

        //public Type GetDDLVisitorType()
        //{
        //    return typeof(MySQLDDLVisitor);
        //}

        //public Type GetTCLVisitorType()
        //{
        //    return typeof(MySQLTCLVisitor);
        //}

        //public Type GetDCLVisitorType()
        //{
        //    return typeof(MySQLDCLVisitor);
        //}

        //public Type GetDALVisitorType()
        //{
        //    return typeof(MySQLDALVisitor);
        //}

        //public Type GetRLVisitorType()
        //{
        //    return typeof(MySQLRLVisitor);
        //}
        public IDMLVisitor CreateDMLVisitor()
        {
            return new MySQLDMLVisitor();
        }

        public IDDLVisitor CreateDDLVisitor()
        {
            return new MySQLDDLVisitor();
        }

        public ITCLVisitor CreateTCLVisitor()
        {
            return new MySQLTCLVisitor();
        }

        public IDCLVisitor CreateDCLVisitor()
        {
            return new MySQLDCLVisitor();
        }

        public IDALVisitor CreateDALVisitor()
        {
            return new MySQLDALVisitor();
        }

        public IRLVisitor CreateRLVisitor()
        {
            return new MySQLRLVisitor();
        }
    }
}
