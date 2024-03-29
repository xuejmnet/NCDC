﻿using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Abstractions.Visitor;
using NCDC.CommandParser.Abstractions.Visitor.Commands;
using NCDC.MySqlParser.Visitor.Impl;

namespace NCDC.MySqlParser.Visitor
{
    /// <summary>
    /// 
    /// </summary>
    /// Author: xjm
    /// Created: 2022/5/10 9:04:01
    /// Email: 326308290@qq.com
    public sealed class MySqlVisitorCreator: ISqlVisitorCreator
    {
        public IDMLVisitor CreateDMLVisitor()
        {
            return new MySqlDMLVisitor();
        }

        public IDDLVisitor CreateDDLVisitor()
        {
            return new MySqlDDLVisitor();
        }

        public ITCLVisitor CreateTCLVisitor()
        {
            return new MySqlTCLVisitor();
        }

        public IDCLVisitor CreateDCLVisitor()
        {
            return new MySqlDCLVisitor();
        }

        public IDALVisitor CreateDALVisitor()
        {
            return new MySqlDALVisitor();
        }

        public IRLVisitor CreateRLVisitor()
        {
            return new MySqlRLVisitor();
        }
    }
}
