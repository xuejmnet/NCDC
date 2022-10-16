using System.Collections.ObjectModel;
using NCDC.CommandParser.Common.Command.DML;
using NCDC.CommandParser.Common.Segment.DDL.Routine;
using NCDC.CommandParser.Common.Segment.DML.Assignment;
using NCDC.CommandParser.Common.Segment.DML.Column;
using NCDC.CommandParser.Common.Segment.DML.Expr;
using NCDC.CommandParser.Common.Segment.DML.Expr.SubQuery;
using NCDC.CommandParser.Common.Segment.DML.Item;
using NCDC.CommandParser.Common.Segment.DML.Order.Item;
using NCDC.CommandParser.Common.Segment.DML.Predicate;
using NCDC.CommandParser.Common.Segment.Generic;
using NCDC.CommandParser.Common.Segment.Generic.Table;
using NCDC.CommandParser.Dialect.Handler.DML;
using NCDC.Extensions;

namespace NCDC.CommandParser.Common.Extractors;

public sealed class TableExtractor
{
    public ICollection<SimpleTableSegment> RewriteTables { get; } = new LinkedList<SimpleTableSegment>();

    public ICollection<ITableSegment> TableContext { get; } = new LinkedList<ITableSegment>();

    /**
     * Extract table that should be rewritten from select statement.
     *
     * @param selectCommand select statement
     */
    public void ExtractTablesFromSelect(SelectCommand selectCommand)
    {
        if (null != selectCommand.From)
        {
            ExtractTablesFromTableSegment(selectCommand.From);
        }

        if (selectCommand.Where is not null)
        {
            ExtractTablesFromExpression(selectCommand.Where.Expr);
        }

        if (null != selectCommand.Projections)
        {
            ExtractTablesFromProjections(selectCommand.Projections);
        }

        if (selectCommand.GroupBy is not null)
        {
            ExtractTablesFromOrderByItems(selectCommand.GroupBy.GetGroupByItems());
        }

        if (selectCommand.OrderBy is not null)
        {
            ExtractTablesFromOrderByItems(selectCommand.OrderBy.GetOrderByItems());
        }

        var lockSegment = SelectCommandHandler.GetLockSegment(selectCommand);
        if (lockSegment is not null)
        {
            ExtractTablesFromLock(lockSegment);
        }

        if (selectCommand.Combine is not null)
        {
            
            ExtractTablesFromSelect(selectCommand.Combine.SelectCommand);
        }
    }

    private void ExtractTablesFromTableSegment(ITableSegment? tableSegment)
    {
        if (tableSegment is null)
        {
            return;
        }
        if (tableSegment is SimpleTableSegment simpleTableSegment) {
            TableContext.Add(tableSegment);
            RewriteTables.Add(simpleTableSegment);
        }
        if (tableSegment is SubQueryTableSegment subQueryTableSegment) {
            TableContext.Add(tableSegment);
            TableExtractor tableExtractor = new TableExtractor();
            tableExtractor.ExtractTablesFromSelect(subQueryTableSegment.SubQuery.Select);
            RewriteTables.AddAll(tableExtractor.RewriteTables);
        }
        if (tableSegment is JoinTableSegment joinTableSegment) {
            ExtractTablesFromJoinTableSegment(joinTableSegment);
        }
        if (tableSegment is DeleteMultiTableSegment deleteMultiTableSegment) {
            RewriteTables.AddAll(deleteMultiTableSegment.ActualDeleteTables);
            ExtractTablesFromTableSegment(deleteMultiTableSegment.RelationTable);
        }
    }

    private void ExtractTablesFromJoinTableSegment(JoinTableSegment tableSegment)
    {
        ExtractTablesFromTableSegment(tableSegment.Left);
        ExtractTablesFromTableSegment(tableSegment.Right);
        ExtractTablesFromExpression(tableSegment.Condition);
    }

    private void ExtractTablesFromExpression(IExpressionSegment? expressionSegment)
    {
        if (expressionSegment is ColumnSegment columnSegment)
        {
            if (columnSegment.Owner is not null && NeedRewrite(columnSegment.Owner))
            {
                if (columnSegment.Owner is not null &&
                    NeedRewrite(columnSegment.Owner))
                {
                    OwnerSegment ownerSegment = columnSegment.Owner;
                    RewriteTables.Add(new SimpleTableSegment(new TableNameSegment(ownerSegment.StartIndex,
                        ownerSegment.StopIndex, ownerSegment.IdentifierValue)));
                }
            }

            if (expressionSegment is ListExpression listExpression) {
                foreach (var listExpressionItem in listExpression.Items)
                {
                    ExtractTablesFromExpression(listExpressionItem);
                }
            }
            if (expressionSegment is ExistsSubQueryExpression existsSubQueryExpression) {
                ExtractTablesFromSelect(existsSubQueryExpression.SubQuery.Select);
            }
            if (expressionSegment is BetweenExpression betweenExpression) {
                ExtractTablesFromExpression(betweenExpression.Left);
                ExtractTablesFromExpression(betweenExpression.BetweenExpr);
                ExtractTablesFromExpression(betweenExpression.AndExpr);
            }
            if (expressionSegment is InExpression inExpression) {
                ExtractTablesFromExpression(inExpression.Left);
                ExtractTablesFromExpression(inExpression.Right);
            }
            if (expressionSegment is SubQueryExpressionSegment subQueryExpressionSegment) {
                ExtractTablesFromSelect(subQueryExpressionSegment.SubQuery.Select);
            }
            if (expressionSegment is BinaryOperationExpression binaryOperationExpression) {
                ExtractTablesFromExpression(binaryOperationExpression.Left);
                ExtractTablesFromExpression(binaryOperationExpression.Right);
            }
        }
    }

    private void ExtractTablesFromProjections(ProjectionsSegment projections)
        {
            foreach (var projectionsProjection in projections.Projections)
            {
                if (projectionsProjection is SubQueryProjectionSegment subQueryProjectionSegment) {
                    ExtractTablesFromSelect(subQueryProjectionSegment.SubQuery.Select);
                } else if (projectionsProjection is IOwnerAvailable ownerAvailable) {
                    if (ownerAvailable.Owner is not null &&
                        NeedRewrite(ownerAvailable.Owner))
                    {
                        OwnerSegment ownerSegment = ownerAvailable.Owner;
                        RewriteTables.Add(CreateSimpleTableSegment(ownerSegment));
                    }
                } else if (projectionsProjection is ColumnProjectionSegment columnProjectionSegment) {
                    if (columnProjectionSegment.Column.Owner is not null &&
                        NeedRewrite(columnProjectionSegment.Column.Owner))
                    {
                        OwnerSegment ownerSegment = columnProjectionSegment.Column.Owner;
                        RewriteTables.Add(CreateSimpleTableSegment(ownerSegment));
                    }
                } else if (projectionsProjection is AggregationProjectionSegment aggregationProjectionSegment)
                {
                    foreach (var expressionSegment in aggregationProjectionSegment.Parameters)
                    {
                        ExtractTablesFromExpression(expressionSegment);
                    }
                }
            }
        }

        private SimpleTableSegment CreateSimpleTableSegment(OwnerSegment ownerSegment)
        {
            SimpleTableSegment result = new SimpleTableSegment(new TableNameSegment(ownerSegment.StartIndex,
                ownerSegment.StopIndex, ownerSegment.IdentifierValue));
            if (ownerSegment.Owner is not null)
            {
                result.Owner = ownerSegment.Owner;
            }
            return result;
        }

        private void ExtractTablesFromOrderByItems(ICollection<OrderByItemSegment> orderByItems)
        {
            foreach (var orderByItemSegment in orderByItems)
            {
                if (orderByItemSegment is ColumnOrderByItemSegment columnOrderByItemSegment) {
                    var owner = columnOrderByItemSegment.GetColumn().Owner;
                    if (owner is not null && NeedRewrite(owner))
                    {
                        RewriteTables.Add(new SimpleTableSegment(new TableNameSegment(owner.StartIndex,
                            owner.StopIndex, owner.IdentifierValue)));
                    }
                }
            }
        }

        private void ExtractTablesFromLock(LockSegment lockSegment)
        {
            RewriteTables.AddAll(lockSegment.Tables);
        }

        /**
         * Extract table that should be rewritten from delete statement.
         *
         * @param deleteCommand delete statement
         */
        public void ExtractTablesFromDelete(DeleteCommand deleteCommand)
        {
            ExtractTablesFromTableSegment(deleteCommand.Table);
            if (deleteCommand.Where is not null)
            {
                ExtractTablesFromExpression(deleteCommand.Where.Expr);
            }
        }

        /**
         * Extract table that should be rewritten from insert statement.
         *
         * @param insertCommand insert statement
         */
        public void ExtractTablesFromInsert(InsertCommand insertCommand)
        {
            if (insertCommand.Table is not null)
            {
                ExtractTablesFromTableSegment(insertCommand.Table);
            }

            if (insertCommand.GetColumns().IsNotEmpty())
            {
                foreach (var columnSegment in insertCommand.GetColumns())
                {
                    ExtractTablesFromExpression(columnSegment);
                }
            }

            InsertStatementHandler.getOnDuplicateKeyColumnsSegment(insertCommand)
                .ifPresent(each->extractTablesFromAssignmentItems(each.getColumns()));
            if (insertCommand.getInsertSelect().isPresent())
            {
                extractTablesFromSelect(insertCommand.getInsertSelect().get().getSelect());
            }
        }

        private void extractTablesFromAssignmentItems(ICollection<AssignmentSegment> assignmentItems)
        {
            assignmentItems.forEach(each->extractTablesFromColumnSegments(each.getColumns()));
        }

        private void extractTablesFromColumnSegments(ICollection<ColumnSegment> columnSegments)
        {
            columnSegments.forEach(each-> {
                if (each.getOwner().isPresent() && NeedRewrite(each.getOwner().get()))
                {
                    OwnerSegment ownerSegment = each.getOwner().get();
                    rewriteTables.add(new SimpleTableSegment(new TableNameSegment(ownerSegment.getStartIndex(),
                        ownerSegment.getStopIndex(), ownerSegment.getIdentifier())));
                }
            });
        }

        /**
         * Extract table that should be rewritten from update statement.
         *
         * @param updateStatement update statement.
         */
        public void extractTablesFromUpdate(UpdateStatement updateStatement)
        {
            ExtractTablesFromTableSegment(updateStatement.getTable());
            updateStatement.getSetAssignment().getAssignments()
                .forEach(each->extractTablesFromExpression(each.getColumns().get(0)));
            if (updateStatement.getWhere().isPresent())
            {
                ExtractTablesFromExpression(updateStatement.getWhere().get().getExpr());
            }
        }

        /**
         * Check if the table needs to be overwritten.
         *
         * @param owner owner
         * @return boolean
         */
        public bool NeedRewrite(OwnerSegment owner)
        {
            foreach (var tableSegment in TableContext)
            {
                if (owner.IdentifierValue.Value.EqualsIgnoreCase(tableSegment.GetAlias()))
                {
                    return false;
                }
            }

            return true;
        }

        /**
         * Extract the tables that should exist from routine body segment.
         *
         * @param routineBody routine body segment
         * @return the tables that should exist
         */
        public ICollection<SimpleTableSegment> extractExistTableFromRoutineBody(RoutineBodySegment routineBody)
        {
            ICollection<SimpleTableSegment> result = new LinkedList<>();
            for (ValidStatementSegment each :
            routineBody.getValidStatements()) {
                if (each.getAlterTable().isPresent())
                {
                    result.add(each.getAlterTable().get().getTable());
                }

                if (each.getDropTable().isPresent())
                {
                    result.addAll(each.getDropTable().get().getTables());
                }

                if (each.getTruncate().isPresent())
                {
                    result.addAll(each.getTruncate().get().getTables());
                }

                result.addAll(extractExistTableFromDMLStatement(each));
            }
            return result;
        }

        private ICollection<SimpleTableSegment> extractExistTableFromDMLStatement(
            ValidStatementSegment validStatementSegment)
        {
            if (validStatementSegment.getInsert().isPresent())
            {
                ExtractTablesFromInsert(validStatementSegment.getInsert().get());
            }
            else if (validStatementSegment.getReplace().isPresent())
            {
                ExtractTablesFromInsert(validStatementSegment.getReplace().get());
            }
            else if (validStatementSegment.getUpdate().isPresent())
            {
                extractTablesFromUpdate(validStatementSegment.getUpdate().get());
            }
            else if (validStatementSegment.getDelete().isPresent())
            {
                ExtractTablesFromDelete(validStatementSegment.getDelete().get());
            }
            else if (validStatementSegment.getSelect().isPresent())
            {
                extractTablesFromSelect(validStatementSegment.getSelect().get());
            }

            return rewriteTables;
        }

        /**
         * Extract the tables that should not exist from routine body segment.
         *
         * @param routineBody routine body segment
         * @return the tables that should not exist
         */
        public ICollection<SimpleTableSegment> extractNotExistTableFromRoutineBody(RoutineBodySegment routineBody)
        {
            ICollection<SimpleTableSegment> result = new LinkedList<>();
            for (ValidStatementSegment each :
            routineBody.getValidStatements()) {
                Optional<CreateTableStatement> createTable = each.getCreateTable();
                if (createTable.isPresent() && !CreateTableStatementHandler.ifNotExists(createTable.get()))
                {
                    result.add(createTable.get().getTable());
                }
            }
            return result;
        }

        /**
         * Extract table that should be rewritten from sql statement.
         *
         * @param sqlStatement sql statement
         */
        public void extractTablesFromSQLStatement(SQLStatement sqlStatement)
        {
            if (sqlStatement instanceof SelectStatement) {
                extractTablesFromSelect((SelectStatement)sqlStatement);
            } else if (sqlStatement instanceof InsertStatement) {
                ExtractTablesFromInsert((InsertStatement)sqlStatement);
            } else if (sqlStatement instanceof UpdateStatement) {
                extractTablesFromUpdate((UpdateStatement)sqlStatement);
            } else if (sqlStatement instanceof DeleteStatement) {
                ExtractTablesFromDelete((DeleteStatement)sqlStatement);
            }
        }

        /**
         * Extract table that should be rewritten from create view statement.
         * 
         * @param createViewStatement create view statement
         */
        public void extractTablesFromCreateViewStatement(CreateViewStatement createViewStatement)
        {
            tableContext.add(createViewStatement.getView());
            rewriteTables.add(createViewStatement.getView());
            extractTablesFromSelect(createViewStatement.getSelect());
        }
    }