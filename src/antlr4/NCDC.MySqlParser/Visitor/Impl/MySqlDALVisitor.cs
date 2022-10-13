﻿using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Abstractions.Visitor.Commands;
using NCDC.CommandParser.Common.Command.DAL.Dialect;
using NCDC.CommandParser.Common.Segment.DAL;
using NCDC.CommandParser.Common.Segment.Generic;
using NCDC.CommandParser.Common.Segment.Generic.Table;
using NCDC.CommandParser.Common.Value.Identifier;
using NCDC.CommandParser.Common.Value.Literal.Impl;
using NCDC.CommandParser.Dialect.Command.MySql.DAL;


namespace NCDC.MySqlParser.Visitor.Impl
{
    /// <summary>
    /// 
    /// </summary>
    /// Author: xjm
    /// Created: 2022/5/10 9:05:14
    /// Email: 326308290@qq.com
    public sealed class MySqlDALVisitor : MySqlVisitor, IDALVisitor
    {
        public override IASTNode VisitUninstallPlugin(MySqlCommandParser.UninstallPluginContext ctx)
        {
            return new UninstallPluginCommand();
        }


        public override IASTNode VisitShowBinaryLogs(MySqlCommandParser.ShowBinaryLogsContext ctx)
        {
            return new ShowBinaryLogsCommand();
        }


        public override IASTNode VisitShowStatus(MySqlCommandParser.ShowStatusContext ctx)
        {
            return new ShowStatusCommand();
        }


        public override IASTNode VisitShowCreateView(MySqlCommandParser.ShowCreateViewContext ctx)
        {
            return new ShowCreateViewCommand();
        }


        public override IASTNode VisitShowCreateEvent(MySqlCommandParser.ShowCreateEventContext ctx)
        {
            return new ShowCreateEventCommand();
        }


        public override IASTNode VisitShowCreateFunction(MySqlCommandParser.ShowCreateFunctionContext ctx)
        {
            return new ShowCreateFunctionCommand();
        }


        public override IASTNode VisitShowCreateProcedure(MySqlCommandParser.ShowCreateProcedureContext ctx)
        {
            return new ShowCreateProcedureCommand();
        }


        public override IASTNode VisitShowBinlogEvents(MySqlCommandParser.ShowBinlogEventsContext ctx)
        {
            return new ShowBinlogCommand();
        }


        public override IASTNode VisitShowErrors(MySqlCommandParser.ShowErrorsContext ctx)
        {
            return new ShowErrorsCommand();
        }


        public override IASTNode VisitShowWarnings(MySqlCommandParser.ShowWarningsContext ctx)
        {
            return new ShowWarningsCommand();
        }


        public override IASTNode VisitResetStatement(MySqlCommandParser.ResetStatementContext ctx)
        {
            return new ResetCommand();
        }


        public override IASTNode VisitRepairTable(MySqlCommandParser.RepairTableContext ctx)
        {
            return new RepairTableCommand();
        }


        public override IASTNode VisitAnalyzeTable(MySqlCommandParser.AnalyzeTableContext ctx)
        {
            return new MySqlAnalyzeTableCommand();
        }


        public override IASTNode VisitCacheIndex(MySqlCommandParser.CacheIndexContext ctx)
        {
            return new CacheIndexCommand();
        }


        public override IASTNode VisitChecksumTable(MySqlCommandParser.ChecksumTableContext ctx)
        {
            return new ChecksumTableCommand();
        }


        public override IASTNode VisitFlush(MySqlCommandParser.FlushContext ctx)
        {
            return new FlushCommand();
        }


        public override IASTNode VisitKill(MySqlCommandParser.KillContext ctx)
        {
            return new KillCommand();
        }


        public override IASTNode VisitLoadIndexInfo(MySqlCommandParser.LoadIndexInfoContext ctx)
        {
            return new LoadIndexInfoCommand();
        }


        public override IASTNode VisitInstallPlugin(MySqlCommandParser.InstallPluginContext ctx)
        {
            return new InstallPluginCommand();
        }


        public override IASTNode VisitOptimizeTable(MySqlCommandParser.OptimizeTableContext ctx)
        {
            return new OptimizeTableCommand();
        }


        public override IASTNode VisitUse(MySqlCommandParser.UseContext ctx)
        {
            MySqlUseCommand result = new MySqlUseCommand();
            result.SetSchema(((IdentifierValue)Visit(ctx.schemaName())).GetValue());
            return result;
        }


        public override IASTNode VisitDesc(MySqlCommandParser.DescContext ctx)
        {
            DescribeCommand result = new DescribeCommand();
            result.SetTable((SimpleTableSegment)Visit(ctx.tableName()));
            return result;
        }


        public override IASTNode VisitShowDatabases(MySqlCommandParser.ShowDatabasesContext ctx)
        {
            return new ShowDatabasesCommand();
        }


        public override IASTNode VisitShowTables(MySqlCommandParser.ShowTablesContext ctx)
        {
            ShowTablesCommand result = new ShowTablesCommand();
            if (null != ctx.fromSchema())
            {
                result.SetFromSchema((FromSchemaSegment)Visit(ctx.fromSchema()));
            }

            return result;
        }


        public override IASTNode VisitShowTableStatus(MySqlCommandParser.ShowTableStatusContext ctx)
        {
            ShowTableStatusCommand result = new ShowTableStatusCommand();
            if (null != ctx.fromSchema())
            {
                result.SetFromSchema((FromSchemaSegment)Visit(ctx.fromSchema()));
            }

            return result;
        }


        public override IASTNode VisitShowColumns(MySqlCommandParser.ShowColumnsContext ctx)
        {
            ShowColumnsCommand result = new ShowColumnsCommand();
            if (null != ctx.fromTable())
            {
                result.SetTable(((FromTableSegment)Visit(ctx.fromTable())).GetTable());
            }

            if (null != ctx.fromSchema())
            {
                result.SetFromSchema((FromSchemaSegment)Visit(ctx.fromSchema()));
            }

            return result;
        }


        public override IASTNode VisitShowIndex(MySqlCommandParser.ShowIndexContext ctx)
        {
            ShowIndexCommand result = new ShowIndexCommand();
            if (null != ctx.fromSchema())
            {
                MySqlCommandParser.SchemaNameContext schemaNameContext = ctx.fromSchema().schemaName();
                // TODO visitSchemaName
                result.SetSchema(new SchemaSegment(schemaNameContext.Start.StartIndex,
                    schemaNameContext.Stop.StopIndex, (IdentifierValue)Visit(schemaNameContext)));
            }

            if (null != ctx.fromTable())
            {
                result.SetTable(((FromTableSegment)VisitFromTable(ctx.fromTable())).GetTable());
            }

            return result;
        }


        public override IASTNode VisitShowCreateTable(MySqlCommandParser.ShowCreateTableContext ctx)
        {
            ShowCreateTableCommand result = new ShowCreateTableCommand();
            result.SetTable((SimpleTableSegment)Visit(ctx.tableName()));
            return result;
        }


        public override IASTNode VisitFromTable(MySqlCommandParser.FromTableContext ctx)
        {
            FromTableSegment result = new FromTableSegment();
            result.SetTable((SimpleTableSegment)Visit(ctx.tableName()));
            return result;
        }


        public override IASTNode VisitShowOther(MySqlCommandParser.ShowOtherContext ctx)
        {
            return new ShowOtherCommand();
        }


        public override IASTNode VisitSetVariable(MySqlCommandParser.SetVariableContext ctx)
        {
            SetCommand result = new SetCommand();
            if (null != ctx.variable())
            {
                result.SetVariable((VariableSegment)Visit(ctx.variable()));
            }

            return result;
        }


        public override IASTNode VisitVariable(MySqlCommandParser.VariableContext ctx)
        {
            return new VariableSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.GetText());
        }


        public override IASTNode VisitFromSchema(MySqlCommandParser.FromSchemaContext ctx)
        {
            return new FromSchemaSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex);
        }


        public override IASTNode VisitShowLike(MySqlCommandParser.ShowLikeContext ctx)
        {
            StringLiteralValue literalValue = (StringLiteralValue)Visit(ctx.stringLiterals());
            return new ShowLikeSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex,
                literalValue.GetValue());
        }
    }
}