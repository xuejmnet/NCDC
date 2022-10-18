using System.Collections.ObjectModel;
using NCDC.CommandParser.Common.Command;
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
using NCDC.CommandParser.Dialect.Handler.DDL;
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

            var onDuplicateKeyColumnsSegment = InsertCommandHandler.GetOnDuplicateKeyColumnsSegment(insertCommand);
            if (onDuplicateKeyColumnsSegment is not null)
            {
                ExtractTablesFromAssignmentItems(onDuplicateKeyColumnsSegment.Columns);
            }
           
            if (insertCommand.InsertSelect is not null)
            {
                ExtractTablesFromSelect(insertCommand.InsertSelect.Select);
            }
        }

        private void ExtractTablesFromAssignmentItems(ICollection<AssignmentSegment> assignmentItems)
        {
            foreach (var assignmentSegment in assignmentItems)
            {
                ExtractTablesFromColumnSegments(assignmentSegment.GetColumns());
            }
        }

        private void ExtractTablesFromColumnSegments(ICollection<ColumnSegment> columnSegments)
        {
            foreach (var columnSegment in columnSegments)
            {
                if (columnSegment.Owner is not null && NeedRewrite(columnSegment.Owner))
                {
                    OwnerSegment ownerSegment = columnSegment.Owner;
                    RewriteTables.Add(new SimpleTableSegment(new TableNameSegment(ownerSegment.StartIndex,
                        ownerSegment.StopIndex, ownerSegment.IdentifierValue)));
                }
            }
        }

        /**
         * Extract table that should be rewritten from update statement.
         *
         * @param updateCommand update statement.
         */
        public void ExtractTablesFromUpdate(UpdateCommand updateCommand)
        {
            ExtractTablesFromTableSegment(updateCommand.Table);
            foreach (var assignmentAssignment in updateCommand.SetAssignment.Assignments)
            {
                ExtractTablesFromExpression(assignmentAssignment.GetColumns()[0]);
            } if (updateCommand.Where is not null)
            {
                ExtractTablesFromExpression(updateCommand.Where.Expr);
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
        public ICollection<SimpleTableSegment> ExtractExistTableFromRoutineBody(RoutineBodySegment routineBody)
        {
            ICollection<SimpleTableSegment> result = new LinkedList<SimpleTableSegment>();
            foreach (var routineBodyValidCommand in routineBody.ValidCommands)
            {
                var alterTableCommand = routineBodyValidCommand.GetAlterTable();
                if (alterTableCommand is not null)
                {
                    result.Add(alterTableCommand.Table);
                }

                var dropTableCommand = routineBodyValidCommand.GetDropTable();
                if (dropTableCommand is not null)
                {
                    result.AddAll(dropTableCommand.Tables);
                }

                var truncateCommand = routineBodyValidCommand.GetTruncate();
                if (truncateCommand is not null)
                {
                    result.AddAll(truncateCommand.Tables);
                }

                result.AddAll(ExtractExistTableFromDMLCommand(routineBodyValidCommand));
            }
            return result;
        }

        private ICollection<SimpleTableSegment> ExtractExistTableFromDMLCommand(
            ValidCommandSegment validCommandSegment)
        {
            if (validCommandSegment.GetInsert() is not null)
            {
                ExtractTablesFromInsert(validCommandSegment.GetInsert()!);
            }
            else if (validCommandSegment.GetReplace() is not null)
            {
                ExtractTablesFromInsert(validCommandSegment.GetReplace()!);
            }
            else if (validCommandSegment.GetUpdate() is not null)
            {
                ExtractTablesFromUpdate(validCommandSegment.GetUpdate()!);
            }
            else if (validCommandSegment.GetDelete() is not null)
            {
                ExtractTablesFromDelete(validCommandSegment.GetDelete()!);
            }
            else if (validCommandSegment.GetSelect() is not null)
            {
                ExtractTablesFromSelect(validCommandSegment.GetSelect()!);
            }

            return RewriteTables;
        }

        /**
         * Extract the tables that should not exist from routine body segment.
         *
         * @param routineBody routine body segment
         * @return the tables that should not exist
         */
        public IEnumerable<SimpleTableSegment> ExtractNotExistTableFromRoutineBody(RoutineBodySegment routineBody)
        {
            foreach (var routineBodyValidCommand in routineBody.ValidCommands)
            {
                var createTableCommand = routineBodyValidCommand.GetCreateTable();
                if (createTableCommand is not null && !CreateTableCommandHandler.IfNotExists(createTableCommand))
                {
                    yield return createTableCommand.Table;
                }
            }
        }

        /**
         * Extract table that should be rewritten from sql statement.
         *
         * @param sqlStatement sql statement
         */
        public void ExtractTablesFromSQLCommand(ISqlCommand sqlCommand)
        {
            if (sqlCommand is SelectCommand selectCommand) {
                ExtractTablesFromSelect(selectCommand);
            } else if (sqlCommand is InsertCommand insertCommand) {
                ExtractTablesFromInsert(insertCommand);
            } else if (sqlCommand is UpdateCommand updateCommand) {
                ExtractTablesFromUpdate(updateCommand);
            } else if (sqlCommand is DeleteCommand deleteCommand) {
                ExtractTablesFromDelete(deleteCommand);
            }
        }
    }