using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Abstractions.Visitor.Commands;
using NCDC.CommandParser.Common.Segment.DML.Expr;
using NCDC.CommandParser.Dialect.Command.MySql.DML;


namespace NCDC.MySqlParser.Visitor.Impl
{
    /// <summary>
    /// 
    /// </summary>
    /// Author: xjm
    /// Created: 2022/5/10 9:06:59
    /// Email: 326308290@qq.com
    public sealed class MySqlDMLVisitor:MySqlVisitor,IDMLVisitor
    {
        public override IASTNode VisitDoStatement(MySqlCommandParser.DoStatementContext ctx)
        {
            var expressionSegments = ctx.expr().Select(o=>(IExpressionSegment) Visit(o)).ToList();
            return new MySqlDoCommand(expressionSegments);
        }
    }
}
