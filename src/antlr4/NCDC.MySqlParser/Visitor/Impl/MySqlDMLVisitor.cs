using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Abstractions.Visitor.Commands;
using NCDC.CommandParser.Command.DML;
using NCDC.CommandParser.Segment.DML;
using NCDC.CommandParser.Segment.DML.Assignment;
using NCDC.CommandParser.Segment.DML.Column;
using NCDC.CommandParser.Segment.DML.Expr;
using NCDC.CommandParser.Segment.DML.Expr.Complex;
using NCDC.CommandParser.Segment.DML.Expr.Simple;
using NCDC.CommandParser.Segment.DML.Expr.SubQuery;
using NCDC.CommandParser.Segment.DML.Item;
using NCDC.CommandParser.Segment.DML.Order;
using NCDC.CommandParser.Segment.DML.Order.Item;
using NCDC.CommandParser.Segment.DML.Pagination;
using NCDC.CommandParser.Segment.DML.Pagination.limit;
using NCDC.CommandParser.Segment.DML.Predicate;
using NCDC.CommandParser.Segment.Generic;
using NCDC.CommandParser.Segment.Generic.Table;
using NCDC.CommandParser.Value.Collection;
using NCDC.CommandParser.Value.Identifier;
using NCDC.CommandParser.Value.Literal.Impl;
using NCDC.CommandParser.Value.ParameterMaker;
using NCDC.Extensions;
using OpenConnector.Extensions;


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
        public override IASTNode VisitCall(MySqlCommandParser.CallContext ctx)
        {
            return new CallCommand();
        }


        public override IASTNode VisitDoStatement(MySqlCommandParser.DoStatementContext ctx)
        {
            return new DoCommand();
        }


        public override IASTNode VisitInsert(MySqlCommandParser.InsertContext ctx)
        {
            // TODO :FIXME, since there is no segment for insertValuesClause, InsertStatement is created by sub rule.
            InsertCommand result;
            if (null != ctx.insertValuesClause())
            {
                result = (InsertCommand)Visit(ctx.insertValuesClause());
            }
            else
            {
                result = new InsertCommand();
                result.SetAssignment = (SetAssignmentSegment)Visit(ctx.setAssignmentsClause());
            }
            if (null != ctx.onDuplicateKeyClause())
            {
                result.OnDuplicateKeyColumns = (OnDuplicateKeyColumnsSegment)Visit(ctx.onDuplicateKeyClause());
            }

            result.Table = (SimpleTableSegment)Visit(ctx.tableName());
            result.SetParameterCount(GetCurrentParameterIndex());
            return result;
        }


        public override IASTNode VisitReplace(MySqlCommandParser.ReplaceContext ctx)
        {
            // TODO :FIXME, since there is no segment for insertValuesClause, InsertStatement is created by sub rule.
            ReplaceCommand result;
            if (null != ctx.insertValuesClause())
            {
                result = (ReplaceCommand)Visit(ctx.insertValuesClause());
            }
            else
            {
                result = new ReplaceCommand();
                result.SetAssignment = (SetAssignmentSegment)Visit(ctx.setAssignmentsClause());
            }

            result.Table = (SimpleTableSegment)Visit(ctx.tableName());
            result.SetParameterCount(GetCurrentParameterIndex());
            return result;
        }


        public override IASTNode VisitInsertValuesClause(MySqlCommandParser.InsertValuesClauseContext ctx)
        {
            InsertCommand result = new InsertCommand();
            if (null != ctx.columnNames())
            {
                MySqlCommandParser.ColumnNamesContext columnNames = ctx.columnNames();
                CollectionValue<ColumnSegment> columnSegments = (CollectionValue<ColumnSegment>)Visit(columnNames);
                result.InsertColumns = new InsertColumnsSegment(columnNames.Start.StartIndex,
                    columnNames.Stop.StopIndex, columnSegments.GetValue());
            }
            else
            {
                result.InsertColumns = new InsertColumnsSegment(ctx.Start.StartIndex - 1, ctx.Start.StartIndex - 1,
                    new List<ColumnSegment>());
            }
            result.Values.AddAll(CreateInsertValuesSegments(ctx.assignmentValues()));
            return result;
        }

        private ICollection<InsertValuesSegment> CreateInsertValuesSegments(ICollection<MySqlCommandParser.AssignmentValuesContext> assignmentValuesContexts)
        {
            ICollection<InsertValuesSegment> result = new LinkedList<InsertValuesSegment>();
            foreach (var each in assignmentValuesContexts)
            {
                result.Add((InsertValuesSegment)Visit(each));
            }
            return result;
        }


        public override IASTNode VisitOnDuplicateKeyClause(MySqlCommandParser.OnDuplicateKeyClauseContext ctx)
        {
            ICollection<AssignmentSegment> columns = new LinkedList<AssignmentSegment>();
            foreach (var each in ctx.assignment())
            {
                columns.Add((AssignmentSegment)Visit(each));
            }
            return new OnDuplicateKeyColumnsSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, columns);
        }


        public override IASTNode VisitUpdate(MySqlCommandParser.UpdateContext ctx)
        {
            UpdateCommand result = new UpdateCommand();
            CollectionValue<TableReferenceSegment> tableReferences = (CollectionValue<TableReferenceSegment>)Visit(ctx.tableReferences());
            foreach (TableReferenceSegment each in tableReferences.GetValue())
            {
                result.Tables.AddAll(each.GetTables());
            }

            result.SetAssignment = (SetAssignmentSegment)Visit(ctx.setAssignmentsClause());
            if (null != ctx.whereClause())
            {
                result.Where = (WhereSegment)Visit(ctx.whereClause());
            }
            result.SetParameterCount(GetCurrentParameterIndex());
            return result;
        }


        public override IASTNode VisitSetAssignmentsClause(MySqlCommandParser.SetAssignmentsClauseContext ctx)
        {
            ICollection<AssignmentSegment> assignments = new LinkedList<AssignmentSegment>();
            foreach (MySqlCommandParser.AssignmentContext each in ctx.assignment())
            {
                assignments.Add((AssignmentSegment)Visit(each));
            }
            return new SetAssignmentSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, assignments);
        }


        public override IASTNode VisitAssignmentValues(MySqlCommandParser.AssignmentValuesContext ctx)
        {
            List<IExpressionSegment> segments = new List<IExpressionSegment>(ctx.assignmentValue().Length);
            foreach (var each in ctx.assignmentValue())
            {
                segments.Add((IExpressionSegment)Visit(each));
            }
            return new InsertValuesSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, segments);
        }


        public override IASTNode VisitAssignment(MySqlCommandParser.AssignmentContext ctx)
        {
            ColumnSegment column = (ColumnSegment)VisitColumnName(ctx.columnName());
            IExpressionSegment value = (IExpressionSegment)Visit(ctx.assignmentValue());
            return new AssignmentSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, column, value);
        }


        public override IASTNode VisitAssignmentValue(MySqlCommandParser.AssignmentValueContext ctx)
        {
            MySqlCommandParser.ExprContext expr = ctx.expr();
            if (null != expr)
            {
                IASTNode result = Visit(expr);
                if (result is ColumnSegment)
                {
                    return new CommonExpressionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.GetText());
                }
                else
                {
                    return result;
                }
            }
            return new CommonExpressionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.GetText());
        }


        public override IASTNode VisitBlobValue(MySqlCommandParser.BlobValueContext ctx)
        {
            return new StringLiteralValue(ctx.STRING_().GetText());
        }


        public override IASTNode VisitDelete(MySqlCommandParser.DeleteContext ctx)
        {
            DeleteCommand result = new DeleteCommand();
            if (null != ctx.multipleTablesClause())
            {
                result.Tables.AddAll(((CollectionValue<SimpleTableSegment>)Visit(ctx.multipleTablesClause())).GetValue());
            }
            else
            {
                result.Tables.Add((SimpleTableSegment)Visit(ctx.singleTableClause()));
            }
            if (null != ctx.whereClause())
            {
                result.Where = (WhereSegment)Visit(ctx.whereClause());
            }
            result.SetParameterCount(GetCurrentParameterIndex());
            return result;
        }


        public override IASTNode VisitSingleTableClause(MySqlCommandParser.SingleTableClauseContext ctx)
        {
            SimpleTableSegment result = (SimpleTableSegment)Visit(ctx.tableName());
            if (null != ctx.alias())
            {
                result.SetAlias((AliasSegment)Visit(ctx.alias()));
            }
            return result;
        }


        public override IASTNode VisitMultipleTablesClause(MySqlCommandParser.MultipleTablesClauseContext ctx)
        {
            CollectionValue<SimpleTableSegment> result = new CollectionValue<SimpleTableSegment>();
            result.Combine((CollectionValue<SimpleTableSegment>)Visit(ctx.multipleTableNames()));
            CollectionValue<TableReferenceSegment> tableReferences = (CollectionValue<TableReferenceSegment>)Visit(ctx.tableReferences());

            foreach (TableReferenceSegment each in tableReferences.GetValue())
            {
                result.GetValue().AddAll(each.GetTables());
            }
            return result;
        }


        public override IASTNode VisitMultipleTableNames(MySqlCommandParser.MultipleTableNamesContext ctx)
        {
            CollectionValue<SimpleTableSegment> result = new CollectionValue<SimpleTableSegment>();
            foreach (var each in ctx.tableName())
            {
                result.GetValue().Add((SimpleTableSegment)Visit(each));
            }
            return result;
        }


        public override IASTNode VisitSelect(MySqlCommandParser.SelectContext ctx)
        {
            // TODO :Unsupported for withClause.
            SelectCommand result = (SelectCommand)Visit(ctx.unionClause());
            result.SetParameterCount(GetCurrentParameterIndex());
            return result;
        }


        public override IASTNode VisitUnionClause(MySqlCommandParser.UnionClauseContext ctx)
        {
            // TODO :Unsupported for union SQL.
            return Visit(ctx.selectClause(0));
        }


        public override IASTNode VisitSelectClause(MySqlCommandParser.SelectClauseContext ctx)
        {
            SelectCommand result = new SelectCommand();
            result.Projections = (ProjectionsSegment)Visit(ctx.projections());
            if (null != ctx.selectSpecification())
            {
                result.Projections.SetDistinctRow(IsDistinct(ctx));
            }
            if (null != ctx.fromClause())
            {
                CollectionValue<TableReferenceSegment> tableReferences = (CollectionValue<TableReferenceSegment>)Visit(ctx.fromClause());
                foreach (TableReferenceSegment each in tableReferences.GetValue())
                {
                    result.TableReferences.Add(each);
                }
            }
            if (null != ctx.whereClause())
            {
                result.Where = (WhereSegment)Visit(ctx.whereClause());
            }
            if (null != ctx.groupByClause())
            {
                result.GroupBy = (GroupBySegment)Visit(ctx.groupByClause());
            }
            if (null != ctx.orderByClause())
            {
                result.OrderBy = (OrderBySegment)Visit(ctx.orderByClause());
            }
            if (null != ctx.limitClause())
            {
                result.Limit = (LimitSegment)Visit(ctx.limitClause());
            }
            if (null != ctx.lockClause())
            {
                result.Lock = (LockSegment)Visit(ctx.lockClause());
            }
            return result;
        }

        private ICollection<SimpleTableSegment> GetTableSegments(Collection<SimpleTableSegment> tableSegments, MySqlCommandParser.JoinedTableContext joinedTable)
        {
            ICollection<SimpleTableSegment> result = new LinkedList<SimpleTableSegment>();
            foreach (SimpleTableSegment tableSegment in ((CollectionValue<SimpleTableSegment>)Visit(joinedTable)).GetValue())
            {
                if (IsTable(tableSegment, tableSegments))
                {
                    result.Add(tableSegment);
                }
            }
            return result;
        }

        private bool IsTable(SimpleTableSegment owner, Collection<SimpleTableSegment> tableSegments)
        {
            foreach (SimpleTableSegment each in tableSegments)
            {
                if (owner.GetTableName().GetIdentifier().GetValue().Equals(each.GetAlias()))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsDistinct(MySqlCommandParser.SelectClauseContext ctx)
        {
            foreach (MySqlCommandParser.SelectSpecificationContext each in ctx.selectSpecification())
            {
                if (((BooleanLiteralValue)Visit(each)).GetValue())
                {
                    return true;
                }
            }
            return false;
        }


        public override IASTNode VisitSelectSpecification(MySqlCommandParser.SelectSpecificationContext ctx)
        {
            if (null != ctx.duplicateSpecification())
            {
                return Visit(ctx.duplicateSpecification());
            }
            return new BooleanLiteralValue(false);
        }


        public override IASTNode VisitDuplicateSpecification(MySqlCommandParser.DuplicateSpecificationContext ctx)
        {
            String text = ctx.GetText();
            if ("DISTINCT".EqualsIgnoreCase(text) || "DISTINCTROW".EqualsIgnoreCase(text))
            {
                return new BooleanLiteralValue(true);
            }
            return new BooleanLiteralValue(false);
        }


        public override IASTNode VisitProjections(MySqlCommandParser.ProjectionsContext ctx)
        {
            ICollection<IProjectionSegment> projections = new LinkedList<IProjectionSegment>();
            if (null != ctx.unqualifiedShorthand())
            {
                projections.Add(new ShorthandProjectionSegment(ctx.unqualifiedShorthand().Start.StartIndex, ctx.unqualifiedShorthand().Stop.StopIndex));
            }
            foreach (var each in ctx.projection())
            {
                projections.Add((IProjectionSegment)Visit(each));
            }
            ProjectionsSegment result = new ProjectionsSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex);
            result.GetProjections().AddAll(projections);
            return result;
        }


        public override IASTNode VisitProjection(MySqlCommandParser.ProjectionContext ctx)
        {
            // FIXME :The stop index of project is the stop index of projection, instead of alias.
            if (null != ctx.qualifiedShorthand())
            {
                MySqlCommandParser.QualifiedShorthandContext shorthand = ctx.qualifiedShorthand();
                ShorthandProjectionSegment result = new ShorthandProjectionSegment(shorthand.Start.StartIndex, shorthand.Stop.StopIndex);
                IdentifierValue identifier = new IdentifierValue(shorthand.identifier().GetText());
                result.SetOwner(new OwnerSegment(shorthand.identifier().Start.StartIndex, shorthand.identifier().Stop.StopIndex, identifier));
                return result;
            }
            AliasSegment alias = null == ctx.alias() ? null : (AliasSegment)Visit(ctx.alias());
            if (null != ctx.columnName())
            {
                ColumnSegment column = (ColumnSegment)Visit(ctx.columnName());
                ColumnProjectionSegment result = new ColumnProjectionSegment(column);
                result.SetAlias(alias);
                return result;
            }
            return CreateProjection(ctx, alias);
        }


        public override IASTNode VisitAlias(MySqlCommandParser.AliasContext ctx)
        {
            if (null != ctx.identifier())
            {
                return new AliasSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, (IdentifierValue)Visit(ctx.identifier()));
            }
            return new AliasSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, new IdentifierValue(ctx.STRING_().GetText()));
        }

        private IASTNode CreateProjection(MySqlCommandParser.ProjectionContext ctx, AliasSegment alias)
        {
            IASTNode projection = Visit(ctx.expr());
            if (projection is AggregationProjectionSegment aggregationProjectionSegment)
            {
                aggregationProjectionSegment.SetAlias(alias);
                return projection;
            }
            if (projection is ExpressionProjectionSegment expressionProjection)
            {
                expressionProjection.SetAlias(alias);
                return projection;
            }
            if (projection is CommonExpressionSegment commonExpressionSegment)
            {
                ExpressionProjectionSegment r = new ExpressionProjectionSegment(commonExpressionSegment.GetStartIndex(), commonExpressionSegment.GetStopIndex(), commonExpressionSegment.GetText());
                r.SetAlias(alias);
                return r;
            }
            // FIXME :For DISTINCT()
            if (projection is ColumnSegment)
            {
                ExpressionProjectionSegment r = new ExpressionProjectionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.GetText());
                r.SetAlias(alias);
                return r;
            }
            if (projection is SubQueryExpressionSegment subQueryExpressionSegment)
            {
                SubQueryProjectionSegment r = new SubQueryProjectionSegment(subQueryExpressionSegment.SubQuery);
                r.SetAlias(alias);
                return r;
            }
            LiteralExpressionSegment column = (LiteralExpressionSegment)projection;
            ExpressionProjectionSegment result = null == alias ? new ExpressionProjectionSegment(column.GetStartIndex(), column.GetStopIndex(), $"{column.GetLiterals()}")
                    : new ExpressionProjectionSegment(column.GetStartIndex(), ctx.alias().Stop.StopIndex, $"{column.GetLiterals()}");
            result.SetAlias(alias);
            return result;
        }


        public override IASTNode VisitFromClause(MySqlCommandParser.FromClauseContext ctx)
        {
            return Visit(ctx.tableReferences());
        }



        public override IASTNode VisitTableReferences(MySqlCommandParser.TableReferencesContext ctx)
        {
            CollectionValue<TableReferenceSegment> result = new CollectionValue<TableReferenceSegment>();
            foreach (MySqlCommandParser.EscapedTableReferenceContext each in ctx.escapedTableReference())
            {
                result.GetValue().Add((TableReferenceSegment)Visit(each));
            }
            return result;
        }


        public override IASTNode VisitEscapedTableReference(MySqlCommandParser.EscapedTableReferenceContext ctx)
        {
            return Visit(ctx.tableReference());
        }


        public override IASTNode VisitTableReference(MySqlCommandParser.TableReferenceContext ctx)
        {
            TableReferenceSegment result = new TableReferenceSegment();
            if (null != ctx.tableFactor())
            {
                TableFactorSegment tableFactor = (TableFactorSegment)Visit(ctx.tableFactor());
                result.SetTableFactor(tableFactor);
            }
            if (!ctx.joinedTable().IsEmpty())
            {
                foreach (MySqlCommandParser.JoinedTableContext each in ctx.joinedTable())
                {
                    JoinedTableSegment joinedTableSegment = (JoinedTableSegment)Visit(each);
                    result.JoinedTables.Add(joinedTableSegment);
                }
            }
            return result;
        }


        public override IASTNode VisitTableFactor(MySqlCommandParser.TableFactorContext ctx)
        {
            TableFactorSegment result = new TableFactorSegment();
            if (null != ctx.subquery())
            {
                SelectCommand subquery = (SelectCommand)Visit(ctx.subquery());
                SubQuerySegment subquerySegment = new SubQuerySegment(ctx.subquery().Start.StartIndex, ctx.subquery().Stop.StopIndex, subquery);
                SubQueryTableSegment subqueryTableSegment = new SubQueryTableSegment(subquerySegment);
                if (null != ctx.alias())
                {
                    subqueryTableSegment.SetAlias((AliasSegment)Visit(ctx.alias()));
                }
                result.SetTable(subqueryTableSegment);
            }
            if (null != ctx.tableName())
            {
                SimpleTableSegment table = (SimpleTableSegment)Visit(ctx.tableName());
                if (null != ctx.alias())
                {
                    table.SetAlias((AliasSegment)Visit(ctx.alias()));
                }
                result.SetTable(table);
            }
            if (null != ctx.tableReferences())
            {
                CollectionValue<TableReferenceSegment> tableReferences = (CollectionValue<TableReferenceSegment>)Visit(ctx.tableReferences());
                result.TableReferences.AddAll(tableReferences.GetValue());
            }
            return result;
        }

        public override IASTNode VisitJoinedTable(MySqlCommandParser.JoinedTableContext ctx)
        {
            JoinedTableSegment result = new JoinedTableSegment();
            TableFactorSegment tableFactor = (TableFactorSegment)Visit(ctx.tableFactor());
            result.SetTableFactor(tableFactor);
            if (null != ctx.joinSpecification())
            {
                result.SetJoinSpecification((JoinSpecificationSegment)Visit(ctx.joinSpecification()));
            }
            return result;
        }


        public override IASTNode VisitJoinSpecification(MySqlCommandParser.JoinSpecificationContext ctx)
        {
            JoinSpecificationSegment result = new JoinSpecificationSegment();
            if (null != ctx.expr())
            {
                IASTNode expr = Visit(ctx.expr());
                if (expr is PredicateSegment)
                {
                    PredicateSegment predicate = (PredicateSegment)expr;
                    result.SetPredicateSegment(predicate);
                }
            }
            if (null != ctx.USING())
            {
                ICollection<ColumnSegment> columnSegmentList = new List<ColumnSegment>();
                foreach (var cname in ctx.columnNames().columnName())
                {
                    columnSegmentList.Add((ColumnSegment)Visit(cname));
                }
                result.SetUsingColumns(columnSegmentList);
            }
            return result;
        }

        private SimpleTableSegment CreateTableSegment(OwnerSegment ownerSegment)
        {
            return new SimpleTableSegment(ownerSegment.GetStartIndex(), ownerSegment.GetStopIndex(), ownerSegment.GetIdentifier());
        }


        public override IASTNode VisitWhereClause(MySqlCommandParser.WhereClauseContext ctx)
        {
            WhereSegment result = new WhereSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex);
            IASTNode segment = Visit(ctx.expr());
            if (segment is OrPredicateSegment orPredicateSegment)
            {
                result.GetAndPredicates().AddAll(orPredicateSegment.GetAndPredicates());
            }
            else if (segment is PredicateSegment predicateSegment)
            {
                AndPredicateSegment andPredicate = new AndPredicateSegment();
                andPredicate.GetPredicates().Add(predicateSegment);
                result.GetAndPredicates().Add(andPredicate);
            }
            return result;
        }


        public override IASTNode VisitGroupByClause(MySqlCommandParser.GroupByClauseContext ctx)
        {
            ICollection<OrderByItemSegment> items = new LinkedList<OrderByItemSegment>();
            foreach (MySqlCommandParser.OrderByItemContext each in ctx.orderByItem())
            {
                items.Add((OrderByItemSegment)Visit(each));
            }
            return new GroupBySegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, items);
        }


        public override IASTNode VisitLimitClause(MySqlCommandParser.LimitClauseContext ctx)
        {
            if (null == ctx.limitOffset())
            {
                return new LimitSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, null, (IPaginationValueSegment)Visit(ctx.limitRowCount()));
            }
            IPaginationValueSegment rowCount;
            IPaginationValueSegment offset;
            if (null != ctx.OFFSET())
            {
                rowCount = (IPaginationValueSegment)Visit(ctx.limitRowCount());
                offset = (IPaginationValueSegment)Visit(ctx.limitOffset());
            }
            else
            {
                offset = (IPaginationValueSegment)Visit(ctx.limitOffset());
                rowCount = (IPaginationValueSegment)Visit(ctx.limitRowCount());
            }
            return new LimitSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, offset, rowCount);
        }


        public override IASTNode VisitLimitRowCount(MySqlCommandParser.LimitRowCountContext ctx)
        {
            if (null != ctx.numberLiterals())
            {
                return new NumberLiteralLimitValueSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, (long)((NumberLiteralValue)Visit(ctx.numberLiterals())).GetValue());
            }

            return new ParameterMarkerLimitValueSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ((ParameterMarkerValue)Visit(ctx.parameterMarker())).GetValue(), ctx.GetText());
        }


        public override IASTNode VisitLimitOffset(MySqlCommandParser.LimitOffsetContext ctx)
        {
            if (null != ctx.numberLiterals())
            {
                return new NumberLiteralLimitValueSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, (long)((NumberLiteralValue)Visit(ctx.numberLiterals())).GetValue());
            }
            return new ParameterMarkerLimitValueSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ((ParameterMarkerValue)Visit(ctx.parameterMarker())).GetValue(), ctx.GetText());
        }


        public override IASTNode VisitSubquery(MySqlCommandParser.SubqueryContext ctx)
        {
            return Visit(ctx.unionClause());
        }


        public override IASTNode VisitLockClause(MySqlCommandParser.LockClauseContext ctx)
        {
            return new LockSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex);
        }
    }
}
