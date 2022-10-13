using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Abstractions.Visitor.Commands;
using NCDC.CommandParser.Common.Command.DML;
using NCDC.CommandParser.Common.Segment.DML;
using NCDC.CommandParser.Common.Segment.DML.Assignment;
using NCDC.CommandParser.Common.Segment.DML.Column;
using NCDC.CommandParser.Common.Segment.DML.Expr;
using NCDC.CommandParser.Common.Segment.DML.Expr.Complex;
using NCDC.CommandParser.Common.Segment.DML.Expr.Simple;
using NCDC.CommandParser.Common.Segment.DML.Expr.SubQuery;
using NCDC.CommandParser.Common.Segment.DML.Item;
using NCDC.CommandParser.Common.Segment.DML.Order;
using NCDC.CommandParser.Common.Segment.DML.Order.Item;
using NCDC.CommandParser.Common.Segment.DML.Predicate;
using NCDC.CommandParser.Common.Segment.Generic;
using NCDC.CommandParser.Common.Segment.Generic.Table;
using NCDC.CommandParser.Common.Value.Collection;
using NCDC.CommandParser.Common.Value.Identifier;
using NCDC.CommandParser.Common.Value.Literal.Impl;
using NCDC.Extensions;
using NCDC.Extensions;


namespace NCDC.SqlServerParser.Visitor.Impl
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/21 11:36:20
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SqlServerDMLVisitor:SqlServerVisitor,IDMLVisitor
    {
        public override IASTNode VisitInsert(SqlServerCommandParser.InsertContext context)
        {
            InsertCommand result = (InsertCommand)Visit(context.insertValuesClause());
            result.Table= (SimpleTableSegment)Visit(context.tableName());
            result.SetParameterCount(GetCurrentParameterIndex());
            return result;
        }

        public override IASTNode VisitInsertValuesClause(SqlServerCommandParser.InsertValuesClauseContext context)
        {
            InsertCommand result = new InsertCommand();
            if (null != context.columnNames())
            {
                SqlServerCommandParser.ColumnNamesContext columnNames = context.columnNames();
                CollectionValue<ColumnSegment> columnSegments = (CollectionValue<ColumnSegment>)Visit(columnNames);
                result.InsertColumns= new InsertColumnsSegment(columnNames.Start.StartIndex, columnNames.Stop.StopIndex, columnSegments.GetValue());
            }
            else
            {
                result.InsertColumns= new InsertColumnsSegment(context.Start.StartIndex - 1, context.Stop.StopIndex - 1, new List<ColumnSegment>());
            }
            result.Values.AddAll(CreateInsertValuesSegments(context.assignmentValues()));
            return result;
        }
        private ICollection<InsertValuesSegment> CreateInsertValuesSegments(ICollection<SqlServerCommandParser.AssignmentValuesContext> assignmentValuesContexts)
        {
            ICollection<InsertValuesSegment> result = new LinkedList<InsertValuesSegment>();
            foreach (var assignmentValuesContext in assignmentValuesContexts)
            {
                result.Add((InsertValuesSegment)Visit(assignmentValuesContext));
            }
            return result;
        }

        public override IASTNode VisitUpdate(SqlServerCommandParser.UpdateContext context)
        {
            UpdateCommand result = new UpdateCommand();
            CollectionValue<TableReferenceSegment> tableReferences = (CollectionValue<TableReferenceSegment>)Visit(context.tableReferences());
            foreach (var tableReferenceSegment in tableReferences.GetValue())
            {
                result.Tables.AddAll(tableReferenceSegment.GetTables());
            }
            result.SetAssignment=(SetAssignmentSegment)Visit(context.setAssignmentsClause());
            if (null != context.whereClause())
            {
                result.Where= (WhereSegment)Visit(context.whereClause());
            }
            result.SetParameterCount(GetCurrentParameterIndex());
            return result;
        }

        public override IASTNode VisitSetAssignmentsClause(SqlServerCommandParser.SetAssignmentsClauseContext context)
        {
            ICollection<AssignmentSegment> assignments = new LinkedList<AssignmentSegment>();
            foreach (var assignmentContext in context.assignment())
            {
                assignments.Add((AssignmentSegment)Visit(assignmentContext));
            }
            return new SetAssignmentSegment(context.Start.StartIndex, context.Stop.StopIndex, assignments);
        }

        public override IASTNode VisitAssignmentValues(SqlServerCommandParser.AssignmentValuesContext context)
        {
            ICollection<IExpressionSegment> segments = new LinkedList<IExpressionSegment>();

            foreach (var assignmentValueContext in context.assignmentValue())
            {
                segments.Add((IExpressionSegment)Visit(assignmentValueContext));
            }
            return new InsertValuesSegment(context.Start.StartIndex, context.Stop.StopIndex, segments);
        }

        public override IASTNode VisitAssignment(SqlServerCommandParser.AssignmentContext context)
        {
            ColumnSegment column = (ColumnSegment)VisitColumnName(context.columnName());
            var value = (IExpressionSegment)Visit(context.assignmentValue());
            return new AssignmentSegment(context.Start.StartIndex, context.Stop.StopIndex, column, value);
        }

        public override IASTNode VisitAssignmentValue(SqlServerCommandParser.AssignmentValueContext context)
        {
            var expr = context.expr();
            if (null != expr)
            {
                return Visit(expr);
            }
            return new CommonExpressionSegment(context.Start.StartIndex, context.Stop.StopIndex, context.GetText());
        }

        public override IASTNode VisitDelete(SqlServerCommandParser.DeleteContext context)
        {
            DeleteCommand result = new DeleteCommand();
            if (null != context.multipleTablesClause())
            {
                result.Tables.AddAll(((CollectionValue<SimpleTableSegment>)Visit(context.multipleTablesClause())).GetValue());
            }
            else
            {
                result.Tables.Add((SimpleTableSegment)Visit(context.singleTableClause()));
            }
            if (null != context.whereClause())
            {
                result.Where= (WhereSegment)Visit(context.whereClause());
            }
            result.SetParameterCount(GetCurrentParameterIndex());
            return result;
        }

        public override IASTNode VisitSingleTableClause(SqlServerCommandParser.SingleTableClauseContext context)
        {
            SimpleTableSegment result = (SimpleTableSegment)Visit(context.tableName());
            if (null != context.alias())
            {
                result.SetAlias((AliasSegment)Visit(context.alias()));
            }
            return result;
        }

        public override IASTNode VisitMultipleTablesClause(SqlServerCommandParser.MultipleTablesClauseContext context)
        {
            CollectionValue<SimpleTableSegment> result = new CollectionValue<SimpleTableSegment>();
            result.Combine((CollectionValue<SimpleTableSegment>)Visit(context.multipleTableNames()));
            CollectionValue<TableReferenceSegment> tableReferences = (CollectionValue<TableReferenceSegment>)Visit(context.tableReferences());
            foreach (var tableReferenceSegment in tableReferences.GetValue())
            {
                result.GetValue().AddAll(tableReferenceSegment.GetTables());
            }
            return result;
        }

        public override IASTNode VisitMultipleTableNames(SqlServerCommandParser.MultipleTableNamesContext context)
        {
            CollectionValue<SimpleTableSegment> result = new CollectionValue<SimpleTableSegment>();
            foreach (var tableNameContext in context.tableName())
            {
                result.GetValue().Add((SimpleTableSegment)Visit(tableNameContext));
            }

            return result;
        }

        public override IASTNode VisitSelect(SqlServerCommandParser.SelectContext context)
        {
            // TODO :Unsupported for withClause.
            var result = (SelectCommand)Visit(context.unionClause());
            result.SetParameterCount(GetCurrentParameterIndex());
            return result;
        }

        public override IASTNode VisitUnionClause(SqlServerCommandParser.UnionClauseContext context)
        {
            // TODO :Unsupported for union SQL.
            return Visit(context.selectClause(0));
        }

        public override IASTNode VisitSelectClause(SqlServerCommandParser.SelectClauseContext context)
        {
            var result = new SelectCommand();
            result.Projections= (ProjectionsSegment)Visit(context.projections());
            if (null != context.duplicateSpecification())
            {
                result.Projections.SetDistinctRow(IsDistinct(context));
            }
            if (null != context.fromClause())
            {
                CollectionValue<TableReferenceSegment> tableReferences = (CollectionValue<TableReferenceSegment>)Visit(context.fromClause());
                foreach (var tableReferenceSegment in tableReferences.GetValue())
                {
                    result.TableReferences.Add(tableReferenceSegment);
                }
            }
            if (null != context.whereClause())
            {
                result.Where= (WhereSegment)Visit(context.whereClause());
            }
            if (null != context.groupByClause())
            {
                result.GroupBy= (GroupBySegment)Visit(context.groupByClause());
            }
            if (null != context.orderByClause())
            {
                result.OrderBy= (OrderBySegment)Visit(context.orderByClause());
            }
            return result;
        }
        private bool IsDistinct(SqlServerCommandParser.SelectClauseContext context)
        {
            return ((BooleanLiteralValue)Visit(context.duplicateSpecification())).GetValue();
        }

        private ICollection<SimpleTableSegment> GetTableSegments(ICollection<SimpleTableSegment> tableSegments, SqlServerCommandParser.JoinedTableContext joinedTable)
        {
            ICollection<SimpleTableSegment> result = new LinkedList<SimpleTableSegment>();
          
            foreach (var simpleTableSegment in ((CollectionValue<SimpleTableSegment>)Visit(joinedTable)).GetValue())
            {

                if (IsTable(simpleTableSegment, tableSegments))
                {
                    result.Add(simpleTableSegment);
                }
            }
            return result;
        }

        private bool IsTable(SimpleTableSegment owner, ICollection<SimpleTableSegment> tableSegments)
        {
            foreach (var simpleTableSegment in tableSegments)
            {
                if (owner.GetTableName().GetIdentifier().GetValue().Equals(simpleTableSegment.GetAlias()))
                {
                    return false;
                }
            }
            return true;
        }

        public override IASTNode VisitDuplicateSpecification(SqlServerCommandParser.DuplicateSpecificationContext context)
        {
            return new BooleanLiteralValue(null != context.DISTINCT());
        }

        public override IASTNode VisitProjections(SqlServerCommandParser.ProjectionsContext context)
        {
            ICollection<IProjectionSegment> projections = new LinkedList<IProjectionSegment>();
            if (null != context.unqualifiedShorthand())
            {
                projections.Add(new ShorthandProjectionSegment(context.unqualifiedShorthand().Start.StartIndex, context.unqualifiedShorthand().Stop.StopIndex));
            }
            foreach (var projectionContext in context.projection())
            {
                projections.Add((IProjectionSegment)Visit(projectionContext));
            }
            ProjectionsSegment result = new ProjectionsSegment(context.Start.StartIndex, context.Stop.StopIndex);
            result.GetProjections().AddAll(projections);
            return result;
        }

        public override IASTNode VisitProjection(SqlServerCommandParser.ProjectionContext context)
        {
            // FIXME :The stop index of project is the stop index of projection, instead of alias.
            if (null != context.qualifiedShorthand())
            {
                var shorthand = context.qualifiedShorthand();
                var result = new ShorthandProjectionSegment(shorthand.Start.StartIndex, shorthand.Stop.StopIndex);
                IdentifierValue identifier = new IdentifierValue(shorthand.identifier().GetText());
                result.SetOwner(new OwnerSegment(shorthand.identifier().Start.StartIndex, shorthand.identifier().Stop.StopIndex, identifier));
                return result;
            }
            AliasSegment alias = null == context.alias() ? null : (AliasSegment)Visit(context.alias());
            if (null != context.columnName())
            {
                ColumnSegment column = (ColumnSegment)Visit(context.columnName());
                ColumnProjectionSegment result = new ColumnProjectionSegment(column);
                result.SetAlias(alias);
                return result;
            }
            return CreateProjection(context, alias);
        }
        private IASTNode CreateProjection(SqlServerCommandParser.ProjectionContext context, AliasSegment alias)
        {
            IASTNode projection = Visit(context.expr());
            if (projection is AggregationProjectionSegment aggregationProjectionSegment) {
                aggregationProjectionSegment.SetAlias(alias);
                return projection;
            }
            if (projection is ExpressionProjectionSegment expressionProjectionSegment) {
                expressionProjectionSegment.SetAlias(alias);
                return projection;
            }
            if (projection is CommonExpressionSegment commonExpressionSegment) {
                ExpressionProjectionSegment commonResult = new ExpressionProjectionSegment(commonExpressionSegment.GetStartIndex(), commonExpressionSegment.GetStopIndex(), commonExpressionSegment.GetText());
                commonResult.SetAlias(alias);
                return commonResult;
            }
            // FIXME :For DISTINCT()
            if (projection is ColumnSegment columnSegment) {
                ExpressionProjectionSegment columnResult = new ExpressionProjectionSegment(context.Start.StartIndex, context.Stop.StopIndex, context.GetText());
                columnResult.SetAlias(alias);
                return columnResult;
            }
            if (projection is SubQueryExpressionSegment subQueryExpressionSegment) {
                SubQueryProjectionSegment subQueryResult = new SubQueryProjectionSegment(subQueryExpressionSegment.SubQuery);
                subQueryResult.SetAlias(alias);
                return subQueryResult;
            }
            LiteralExpressionSegment column = (LiteralExpressionSegment)projection;
            ExpressionProjectionSegment result = null == alias ? new ExpressionProjectionSegment(column.GetStartIndex(), column.GetStopIndex(), column.GetLiterals()?.ToString())
                : new ExpressionProjectionSegment(column.GetStartIndex(), context.alias().Stop.StopIndex, column.GetLiterals()?.ToString());
            result.SetAlias(alias);
            return result;
        }

        public override IASTNode VisitAlias(SqlServerCommandParser.AliasContext context)
        {
            if (null != context.identifier())
            {
                return new AliasSegment(context.Start.StartIndex, context.Stop.StopIndex, (IdentifierValue)Visit(context.identifier()));
            }
            return new AliasSegment(context.Start.StartIndex, context.Stop.StopIndex, new IdentifierValue(context.STRING_().GetText()));
        }

        public override IASTNode VisitFromClause(SqlServerCommandParser.FromClauseContext context)
        {
            return Visit(context.tableReferences());
        }

        public override IASTNode VisitTableReferences(SqlServerCommandParser.TableReferencesContext context)
        {
            CollectionValue<TableReferenceSegment> result = new CollectionValue<TableReferenceSegment>();
            foreach (var tableReferenceContext in context.tableReference())
            {
                result.GetValue().Add((TableReferenceSegment)Visit(tableReferenceContext));
            }
            return result;
        }

        public override IASTNode VisitTableReference(SqlServerCommandParser.TableReferenceContext context)
        {
            TableReferenceSegment result = new TableReferenceSegment();
            if (null != context.tableFactor())
            {
                TableFactorSegment tableFactor = (TableFactorSegment)Visit(context.tableFactor());
                result.SetTableFactor(tableFactor);
            }
            if (!context.joinedTable().IsEmpty())
            {
                foreach (var joinedTableContext in context.joinedTable())
                {
                    JoinedTableSegment joinedTableSegment = (JoinedTableSegment)Visit(joinedTableContext);
                    result.JoinedTables.Add(joinedTableSegment);
                }
            }
            return result;
        }

        public override IASTNode VisitTableFactor(SqlServerCommandParser.TableFactorContext context)
        {
            TableFactorSegment result = new TableFactorSegment();
            if (null != context.subquery())
            {
                var subquery = (SelectCommand)Visit(context.subquery());
                var subquerySegment = new SubQuerySegment(context.subquery().Start.StartIndex, context.subquery().Stop.StopIndex, subquery);
                var subqueryTableSegment = new SubQueryTableSegment(subquerySegment);
                if (null != context.alias())
                {
                    subqueryTableSegment.SetAlias((AliasSegment)Visit(context.alias()));
                }
                result.SetTable(subqueryTableSegment);
            }
            if (null != context.tableName())
            {
                SimpleTableSegment table = (SimpleTableSegment)Visit(context.tableName());
                if (null != context.alias())
                {
                    table.SetAlias((AliasSegment)Visit(context.alias()));
                }
                result.SetTable(table);
            }
            if (null != context.tableReferences())
            {
                var tableReferences = (CollectionValue<TableReferenceSegment>)Visit(context.tableReferences());
                result.TableReferences.AddAll(tableReferences.GetValue());
            }
            return result;
        }

        public override IASTNode VisitJoinedTable(SqlServerCommandParser.JoinedTableContext context)
        {
            JoinedTableSegment result = new JoinedTableSegment();
            TableFactorSegment tableFactor = (TableFactorSegment)Visit(context.tableFactor());
            result.SetTableFactor(tableFactor);
            if (null != context.joinSpecification())
            {
                result.SetJoinSpecification((JoinSpecificationSegment)Visit(context.joinSpecification()));
            }
            return result;
        }

        public override IASTNode VisitJoinSpecification(SqlServerCommandParser.JoinSpecificationContext context)
        {
            JoinSpecificationSegment result = new JoinSpecificationSegment();
            if (null != context.expr())
            {
                var expr = Visit(context.expr());
                if (expr is PredicateSegment predicateSegment) {
                    result.SetPredicateSegment(predicateSegment);
                }
            }
            if (null != context.USING())
            {
                ICollection<ColumnSegment> columnSegmentList = new LinkedList<ColumnSegment>();
                foreach (var columnNameWithSortContext in context.columnNames().columnNameWithSort())
                {

                    columnSegmentList.Add((ColumnSegment)Visit(columnNameWithSortContext));
                }
                result.SetUsingColumns(columnSegmentList);
            }
            return result;
        }
        private SimpleTableSegment CreateTableSegment(OwnerSegment ownerSegment)
        {
            return new SimpleTableSegment(ownerSegment.GetStartIndex(), ownerSegment.GetStopIndex(), ownerSegment.GetIdentifier());
        }

        public override IASTNode VisitWhereClause(SqlServerCommandParser.WhereClauseContext context)
        {
            WhereSegment result = new WhereSegment(context.Start.StartIndex, context.Stop.StopIndex);
            var segment = Visit(context.expr());
            if (segment is OrPredicateSegment orPredicateSegment) {
                result.GetAndPredicates().AddAll(orPredicateSegment.GetAndPredicates());
            } else if (segment is PredicateSegment predicateSegment) {
                var andPredicate = new AndPredicateSegment();
                andPredicate.GetPredicates().Add(predicateSegment);
                result.GetAndPredicates().Add(andPredicate);
            }
            return result;
        }

        public override IASTNode VisitGroupByClause(SqlServerCommandParser.GroupByClauseContext context)
        {
            ICollection<OrderByItemSegment> items = new LinkedList<OrderByItemSegment>();
            foreach (var orderByItemContext in context.orderByItem())
            {
                items.Add((OrderByItemSegment)Visit(orderByItemContext));
            }
            return new GroupBySegment(context.Start.StartIndex, context.Stop.StopIndex, items);
        }
    }
}
