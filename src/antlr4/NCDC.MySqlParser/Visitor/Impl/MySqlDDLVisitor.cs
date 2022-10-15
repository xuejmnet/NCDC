using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Abstractions.Visitor.Commands;
using NCDC.CommandParser.Common.Segment.DDL;
using NCDC.CommandParser.Common.Segment.DDL.Charset;
using NCDC.CommandParser.Common.Segment.DDL.Column;
using NCDC.CommandParser.Common.Segment.DDL.Column.Alter;
using NCDC.CommandParser.Common.Segment.DDL.Column.Position;
using NCDC.CommandParser.Common.Segment.DDL.Constraint;
using NCDC.CommandParser.Common.Segment.DDL.Constraint.Alter;
using NCDC.CommandParser.Common.Segment.DDL.Index;
using NCDC.CommandParser.Common.Segment.DDL.Routine;
using NCDC.CommandParser.Common.Segment.DDL.Table;
using NCDC.CommandParser.Common.Segment.DML.Column;
using NCDC.CommandParser.Common.Segment.Generic;
using NCDC.CommandParser.Common.Segment.Generic.Table;
using NCDC.CommandParser.Common.Value.Collection;
using NCDC.CommandParser.Common.Value.Identifier;
using NCDC.CommandParser.Dialect.Command.MySql.DDL;
using NCDC.CommandParser.Dialect.Command.MySql.DML;
using NCDC.Extensions;

namespace NCDC.MySqlParser.Visitor.Impl
{
    /// <summary>
    /// 
    /// </summary>
    /// Author: xjm
    /// Created: 2022/5/10 9:06:32
    /// Email: 326308290@qq.com
    public sealed class MySqlDDLVisitor : MySqlVisitor, IDDLVisitor
    {
        public override IASTNode VisitCreateDatabase(MySqlCommandParser.CreateDatabaseContext ctx)
        {
            return new MySqlCreateDatabaseCommand(new IdentifierValue(ctx.schemaName().GetText()).Value,
                null != ctx.ifNotExists());
        }

        public override IASTNode VisitAlterDatabase(MySqlCommandParser.AlterDatabaseContext ctx)
        {
            return new MySqlAlterDatabaseCommand();
        }

        public override IASTNode VisitDropDatabase(MySqlCommandParser.DropDatabaseContext ctx)
        {
            return new MySqlDropDatabaseCommand(new IdentifierValue(ctx.schemaName().GetText()).Value,
                null != ctx.ifExists());
        }

        public override IASTNode VisitCreateTable(MySqlCommandParser.CreateTableContext ctx)
        {
            MySqlCreateTableCommand result = new MySqlCreateTableCommand(null != ctx.ifNotExists(),
                (SimpleTableSegment)Visit(ctx.tableName()));
            if (null != ctx.createDefinitionClause())
            {
                CollectionValue<ICreateDefinitionSegment> createDefinitions =
                    (CollectionValue<ICreateDefinitionSegment>)Visit(ctx.createDefinitionClause());
                foreach (var createDefinitionSegment in createDefinitions.Value)
                {
                    if (createDefinitionSegment is ColumnDefinitionSegment columnDefinition)
                    {
                        result.ColumnDefinitions.Add(columnDefinition);
                    }
                    else if (createDefinitionSegment is ConstraintDefinitionSegment constraintDefinition)
                    {
                        result.ConstraintDefinitions.Add(constraintDefinition);
                    }
                }
            }

            return result;
        }

        public override IASTNode VisitCreateDefinitionClause(MySqlCommandParser.CreateDefinitionClauseContext ctx)
        {
            CollectionValue<ICreateDefinitionSegment> result = new CollectionValue<ICreateDefinitionSegment>();
            foreach (var tableElementContext in ctx.tableElementList().tableElement())
            {
                if (null != tableElementContext.columnDefinition())
                {
                    result.Value.Add((ColumnDefinitionSegment)Visit(tableElementContext.columnDefinition()));
                }

                if (null != tableElementContext.tableConstraintDef())
                {
                    result.Value.Add((ConstraintDefinitionSegment)Visit(tableElementContext.tableConstraintDef()));
                }
            }

            return result;
        }

        public override IASTNode VisitAlterTable(MySqlCommandParser.AlterTableContext ctx)
        {
            MySqlAlterTableCommand result = new MySqlAlterTableCommand((SimpleTableSegment)Visit(ctx.tableName()));
            if (null != ctx.alterTableActions() && null != ctx.alterTableActions().alterCommandList() &&
                null != ctx.alterTableActions().alterCommandList().alterList())
            {
                var alterDefinitionSegments =
                    ((CollectionValue<IAlterDefinitionSegment>)Visit(ctx.alterTableActions().alterCommandList()
                        .alterList())).Value;
                foreach (var alterDefinitionSegment in alterDefinitionSegments)
                {
                    if (alterDefinitionSegment is AddColumnDefinitionSegment alterDefinition)
                    {
                        result.AddColumnDefinitions.Add(alterDefinition);
                    }
                    else if (alterDefinitionSegment is ModifyColumnDefinitionSegment modifyColumnDefinition)
                    {
                        result.ModifyColumnDefinitions.Add(modifyColumnDefinition);
                    }
                    else if (alterDefinitionSegment is ChangeColumnDefinitionSegment changeColumnDefinition)
                    {
                        result.ChangeColumnDefinitions.Add(changeColumnDefinition);
                    }
                    else if (alterDefinitionSegment is DropColumnDefinitionSegment dropColumnDefinition)
                    {
                        result.DropColumnDefinitions.Add(dropColumnDefinition);
                    }
                    else if (alterDefinitionSegment is AddConstraintDefinitionSegment addConstraintDefinition)
                    {
                        result.AddConstraintDefinitions.Add(addConstraintDefinition);
                    }
                    else if (alterDefinitionSegment is DropConstraintDefinitionSegment dropConstraintDefinition)
                    {
                        result.DropConstraintDefinitions.Add(dropConstraintDefinition);
                    }
                    else if (alterDefinitionSegment is RenameTableDefinitionSegment renameTableDefinition)
                    {
                        result.RenameTable = renameTableDefinition.RenameTable;
                    }
                    else if (alterDefinitionSegment is ConvertTableDefinitionSegment convertTableDefinition)
                    {
                        result.ConvertTableDefinition = convertTableDefinition;
                    }
                    else if (alterDefinitionSegment is DropIndexDefinitionSegment dropIndexDefinition)
                    {
                        result.DropIndexDefinitions.Add(dropIndexDefinition);
                    }
                }
            }

            return result;
        }

        private ColumnDefinitionSegment GenerateColumnDefinitionSegment(ColumnSegment column,
            MySqlCommandParser.FieldDefinitionContext ctx)
        {
            DataTypeSegment dataTypeSegment = (DataTypeSegment)Visit(ctx.dataType());
            var isPrimaryKey = ctx.columnAttribute().Any(o => o.KEY() != null && o.UNIQUE() == null);
            // TODO parse not null
            return new ColumnDefinitionSegment(column.StartIndex, ctx.Stop.StopIndex, column, dataTypeSegment,
                isPrimaryKey, false);
        }

        public override IASTNode VisitAlterConstraint(MySqlCommandParser.AlterConstraintContext ctx)
        {
            return new ModifyConstraintDefinitionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex,
                (ConstraintSegment)Visit(ctx.constraintName()));
        }

        public override IASTNode VisitCharsetName(MySqlCommandParser.CharsetNameContext ctx)
        {
            return new CharsetNameSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, ctx.GetText());
        }

        public override IASTNode VisitAddTableConstraint(MySqlCommandParser.AddTableConstraintContext ctx)
        {
            return new AddConstraintDefinitionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex,
                (ConstraintDefinitionSegment)Visit(ctx.tableConstraintDef()));
        }

        public override IASTNode VisitAlterRenameTable(MySqlCommandParser.AlterRenameTableContext ctx)
        {
            RenameTableDefinitionSegment result =
                new RenameTableDefinitionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex);
            result.RenameTable = (SimpleTableSegment)Visit(ctx.tableName());
            return result;
        }

        public override IASTNode VisitRenameTable(MySqlCommandParser.RenameTableContext ctx)
        {
            MySqlRenameTableCommand result = new MySqlRenameTableCommand();
            for (int i = 0, len = ctx.tableName().Length; i < len; i += 2)
            {
                MySqlCommandParser.TableNameContext tableName = ctx.tableName(i);
                MySqlCommandParser.TableNameContext renameTableName = ctx.tableName(i + 1);
                result.RenameTables.Add(CreateRenameTableDefinitionSegment(tableName, renameTableName));
            }

            return result;
        }

        private RenameTableDefinitionSegment CreateRenameTableDefinitionSegment(
            MySqlCommandParser.TableNameContext tableName, MySqlCommandParser.TableNameContext renameTableName)
        {
            RenameTableDefinitionSegment result =
                new RenameTableDefinitionSegment(tableName.Start.StartIndex, renameTableName.Stop.StopIndex);
            result.Table = (SimpleTableSegment)Visit(tableName);
            result.RenameTable = (SimpleTableSegment)Visit(renameTableName);
            return result;
        }

        private DropColumnDefinitionSegment GenerateDropColumnDefinitionSegment(
            MySqlCommandParser.AlterTableDropContext ctx)
        {
            ColumnSegment column = new ColumnSegment(ctx.columnInternalRef.Start.StartIndex,
                ctx.columnInternalRef.Stop.StopIndex,
                (IdentifierValue)Visit(ctx.columnInternalRef));
            return new DropColumnDefinitionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex,
                new List<ColumnSegment>() { column });
        }

        private ModifyColumnDefinitionSegment GenerateModifyColumnDefinitionSegment(
            MySqlCommandParser.ModifyColumnContext ctx)
        {
            ColumnSegment column = new ColumnSegment(ctx.columnInternalRef.Start.StartIndex,
                ctx.columnInternalRef.Stop.StopIndex, (IdentifierValue)Visit(ctx.columnInternalRef));
            ModifyColumnDefinitionSegment result = new ModifyColumnDefinitionSegment(
                ctx.Start.StartIndex, ctx.Stop.StopIndex,
                GenerateColumnDefinitionSegment(column, ctx.fieldDefinition()));
            if (null != ctx.place())
            {
                result.ColumnPosition = (ColumnPositionSegment)Visit(ctx.place());
            }

            return result;
        }

        private ChangeColumnDefinitionSegment GenerateModifyColumnDefinitionSegment(
            MySqlCommandParser.ChangeColumnContext ctx)
        {
            ChangeColumnDefinitionSegment result = new ChangeColumnDefinitionSegment(ctx.Start.StartIndex,
                ctx.Stop.StopIndex, (ColumnDefinitionSegment)Visit(ctx.columnDefinition()));
            result.PreviousColumn = new ColumnSegment(ctx.columnInternalRef.Start.StartIndex,
                ctx.columnInternalRef.Stop.StopIndex,
                new IdentifierValue(ctx.columnInternalRef.GetText()));
            if (null != ctx.place())
            {
                result.ColumnPosition = (ColumnPositionSegment)Visit(ctx.place());
            }

            return result;
        }

        public override IASTNode VisitAddColumn(MySqlCommandParser.AddColumnContext ctx)
        {
            ICollection<ColumnDefinitionSegment> columnDefinitions = new LinkedList<ColumnDefinitionSegment>();
            if (null != ctx.columnDefinition())
            {
                columnDefinitions.Add((ColumnDefinitionSegment)Visit(ctx.columnDefinition()));
            }

            if (null != ctx.tableElementList())
            {
                foreach (var tableElementContext in ctx.tableElementList().tableElement())
                {
                    if (null != tableElementContext.columnDefinition())
                    {
                        columnDefinitions.Add((ColumnDefinitionSegment)Visit(tableElementContext.columnDefinition()));
                    }
                }
            }

            AddColumnDefinitionSegment result =
                new AddColumnDefinitionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, columnDefinitions);
            if (null != ctx.place())
            {
                if (1 != columnDefinitions.Count)
                {
                    throw new InvalidOperationException();
                }

                result.ColumnPosition = (ColumnPositionSegment)Visit(ctx.place());
            }

            return result;
        }

        public override IASTNode VisitColumnDefinition(MySqlCommandParser.ColumnDefinitionContext ctx)
        {
            ColumnSegment column = new ColumnSegment(ctx.column_name.Start.StartIndex, ctx.column_name.Stop.StopIndex,
                (IdentifierValue)Visit(ctx.column_name));
            DataTypeSegment dataTypeSegment = (DataTypeSegment)Visit(ctx.fieldDefinition().dataType());
            bool isPrimaryKey = ctx.fieldDefinition().columnAttribute().Any(o => o.KEY() != null && o.UNIQUE() == null);
            // TODO parse not null
            ColumnDefinitionSegment result = new ColumnDefinitionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex,
                column, dataTypeSegment, isPrimaryKey, false);
            result.ReferencedTables.AddAll(GetReferencedTables(ctx));
            return result;
        }

        private IEnumerable<SimpleTableSegment> GetReferencedTables(MySqlCommandParser.ColumnDefinitionContext ctx)
        {
            if (null != ctx.referenceDefinition())
            {
                var simpleTableSegment = (SimpleTableSegment)Visit(ctx.referenceDefinition());
                yield return simpleTableSegment;
            }
        }

        public override IASTNode VisitTableConstraintDef(MySqlCommandParser.TableConstraintDefContext ctx)
        {
            ConstraintDefinitionSegment result =
                new ConstraintDefinitionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex);
            if (null != ctx.constraintClause() && null != ctx.constraintClause().constraintName())
            {
                result.ConstraintName = (ConstraintSegment)Visit(ctx.constraintClause().constraintName());
            }

            if (null != ctx.KEY() && null != ctx.PRIMARY())
            {
                result.PrimaryKeyColumns.AddAll(((CollectionValue<ColumnSegment>)Visit(ctx.keyListWithExpression()))
                    .Value);
                return result;
            }

            if (null != ctx.FOREIGN())
            {
                result.ReferencedTable = (SimpleTableSegment)Visit(ctx.referenceDefinition());
                return result;
            }

            if (null != ctx.UNIQUE())
            {
                result.IndexColumns.AddAll(((CollectionValue<ColumnSegment>)Visit(ctx.keyListWithExpression())).Value);
                if (null != ctx.indexName())
                {
                    result.IndexName = (IndexSegment)Visit(ctx.indexName());
                }

                return result;
            }

            if (null != ctx.checkConstraint())
            {
                return result;
            }

            result.IndexColumns.AddAll(((CollectionValue<ColumnSegment>)Visit(ctx.keyListWithExpression())).Value);
            if (null != ctx.indexName())
            {
                result.IndexName = (IndexSegment)Visit(ctx.indexName());
            }

            return result;
        }

        public override IASTNode VisitKeyListWithExpression(MySqlCommandParser.KeyListWithExpressionContext ctx)
        {
            CollectionValue<ColumnSegment> result = new CollectionValue<ColumnSegment>();
            foreach (var keyPartWithExpressionContext in ctx.keyPartWithExpression())
            {
                if (null != keyPartWithExpressionContext.keyPart())
                {
                    result.Value.Add((ColumnSegment)Visit(keyPartWithExpressionContext.keyPart().columnName()));
                }
            }

            return result;
        }

        public override IASTNode VisitReferenceDefinition(MySqlCommandParser.ReferenceDefinitionContext ctx)
        {
            return Visit(ctx.tableName());
        }

        public override IASTNode VisitPlace(MySqlCommandParser.PlaceContext ctx)
        {
            ColumnSegment? columnName = null;
            if (null != ctx.columnName())
            {
                columnName = (ColumnSegment)Visit(ctx.columnName());
            }

            return null == ctx.columnName()
                ? new ColumnFirstPositionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, columnName)
                : new ColumnAfterPositionSegment(ctx.Start.StartIndex, ctx.Stop.StopIndex, columnName);
        }

        public override IASTNode VisitDropTable(MySqlCommandParser.DropTableContext ctx)
        {
            MySqlDropTableCommand result = new MySqlDropTableCommand(null != ctx.ifExists());
            result.Tables.AddAll(((CollectionValue<SimpleTableSegment>)Visit(ctx.tableList())).Value);
            return result;
        }


        public override IASTNode VisitTruncateTable(MySqlCommandParser.TruncateTableContext ctx)
        {
            MySqlTruncateCommand result = new MySqlTruncateCommand();
            result.Tables.Add((SimpleTableSegment)Visit(ctx.tableName()));
            return result;
        }


        public override IASTNode VisitCreateIndex(MySqlCommandParser.CreateIndexContext ctx)
        {
            var table = (SimpleTableSegment)Visit(ctx.tableName());
            IndexNameSegment indexName = new IndexNameSegment(ctx.indexName().Start.StartIndex,
                ctx.indexName().Stop.StopIndex, new IdentifierValue(ctx.indexName().GetText()));
            var index = new IndexSegment(ctx.indexName().Start.StartIndex, ctx.indexName().Stop.StopIndex, indexName);
            MySqlCreateIndexCommand result = new MySqlCreateIndexCommand(index, table);
            result.Columns.AddAll(((CollectionValue<ColumnSegment>)Visit(ctx.keyListWithExpression())).Value);
            return result;
        }


        public override IASTNode VisitDropIndex(MySqlCommandParser.DropIndexContext ctx)
        {
            var table = (SimpleTableSegment)Visit(ctx.tableName());
            MySqlDropIndexCommand result = new MySqlDropIndexCommand(table);
            IndexNameSegment indexName = new IndexNameSegment(ctx.indexName().Start.StartIndex,
                ctx.indexName().Stop.StopIndex, new IdentifierValue(ctx.indexName().GetText()));
            var index = new IndexSegment(ctx.indexName().Start.StartIndex,
                ctx.indexName().Stop.StopIndex, indexName);
            result.Indexes.Add(index);
            return result;
        }

        public override IASTNode VisitCaseStatement(MySqlCommandParser.CaseStatementContext ctx)
        {
            CollectionValue<ValidCommandSegment> result = new CollectionValue<ValidCommandSegment>();
            foreach (var validStatementContext in ctx.validStatement())
            {
                result.Combine((CollectionValue<ValidCommandSegment>)Visit(validStatementContext));
            }

            return result;
        }

        public override IASTNode VisitIfStatement(MySqlCommandParser.IfStatementContext ctx)
        {
            CollectionValue<ValidCommandSegment> result = new CollectionValue<ValidCommandSegment>();
            foreach (var validStatementContext in ctx.validStatement())
            {
                result.Combine((CollectionValue<ValidCommandSegment>)Visit(validStatementContext));
            }

            return result;
        }


    }
}