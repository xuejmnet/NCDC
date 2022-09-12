using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Abstractions.Visitor.Commands;
using NCDC.CommandParser.Command.DDL;
using NCDC.CommandParser.Segment.DDL;
using NCDC.CommandParser.Segment.DDL.Column;
using NCDC.CommandParser.Segment.DDL.Column.Alter;
using NCDC.CommandParser.Segment.DDL.Constraint;
using NCDC.CommandParser.Segment.DDL.Index;
using NCDC.CommandParser.Segment.DML.Column;
using NCDC.CommandParser.Segment.Generic;
using NCDC.CommandParser.Segment.Generic.Table;
using NCDC.CommandParser.Value.Collection;
using NCDC.Extensions;
using OpenConnector.Extensions;


namespace NCDC.SqlServerParser.Visitor.Impl
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/21 8:36:16
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SqlServerDDLVisitor:SqlServerVisitor,IDDLVisitor
    {
        public override IASTNode VisitCreateTable(SqlServerCommandParser.CreateTableContext context)
        {
            CreateTableCommand result = new CreateTableCommand((SimpleTableSegment)Visit(context.tableName()));
            if (null != context.createDefinitionClause())
            {
                CollectionValue<ICreateDefinitionSegment> createDefinitions = (CollectionValue<ICreateDefinitionSegment>)Visit(context.createDefinitionClause());
                foreach (var createDefinitionSegment in createDefinitions.GetValue())
                {
                    if (createDefinitionSegment is ColumnDefinitionSegment columnDefinitionSegment)
                    {
                        result.ColumnDefinitions.Add(columnDefinitionSegment);
                    } else if (createDefinitionSegment is ConstraintDefinitionSegment constraintDefinitionSegment)
                    {
                        result.ConstraintDefinitions.Add(constraintDefinitionSegment);
                    }
                }
            }
            return result;
        }

        public override IASTNode VisitCreateDefinitionClause(SqlServerCommandParser.CreateDefinitionClauseContext context)
        {
            CollectionValue<ICreateDefinitionSegment> result = new CollectionValue<ICreateDefinitionSegment>();
            foreach (var createTableDefinitionContext in context.createTableDefinitions().createTableDefinition())
            {
                if (null != createTableDefinitionContext.columnDefinition())
                {
                    result.GetValue().Add((ColumnDefinitionSegment)Visit(createTableDefinitionContext.columnDefinition()));
                }
                if (null != createTableDefinitionContext.tableConstraint())
                {
                    result.GetValue().Add((ConstraintDefinitionSegment)Visit(createTableDefinitionContext.tableConstraint()));
                }
            }
            return result;
        }

        public override IASTNode VisitColumnDefinition(SqlServerCommandParser.ColumnDefinitionContext context)
        {
            ColumnSegment column = (ColumnSegment)Visit(context.columnName());
            DataTypeSegment dataType = (DataTypeSegment)Visit(context.dataType());
            var isPrimaryKey = IsPrimaryKey(context);
            ColumnDefinitionSegment result = new ColumnDefinitionSegment(
                context.Start.StartIndex, context.Stop.StopIndex, column, dataType, isPrimaryKey);

            foreach (var columnDefinitionOptionContext in context.columnDefinitionOption())
            {
                foreach (var columnConstraintContext in columnDefinitionOptionContext.columnConstraint())
                {
                    if (null != columnConstraintContext.columnForeignKeyConstraint())
                    {
                        result.ReferencedTables.Add((SimpleTableSegment)Visit(columnConstraintContext.columnForeignKeyConstraint().tableName()));
                    }
                }
            }

            foreach (var columnConstraintContext in context.columnConstraints().columnConstraint())
            {
                if (null != columnConstraintContext.columnForeignKeyConstraint())
                {
                    result.ReferencedTables.Add((SimpleTableSegment)Visit(columnConstraintContext.columnForeignKeyConstraint().tableName()));
                }
            }
            return result;
        }
        private bool IsPrimaryKey(SqlServerCommandParser.ColumnDefinitionContext context)
        {
            foreach (var columnDefinitionOptionContext in context.columnDefinitionOption())
            {
                foreach (var columnConstraintContext in columnDefinitionOptionContext.columnConstraint())
                {
                    if (null != columnConstraintContext.primaryKeyConstraint() && null != columnConstraintContext.primaryKeyConstraint().primaryKey())
                    {
                        return true;
                    }
                }
            }

            foreach (var columnConstraintContext in context.columnConstraints().columnConstraint())
            {
                if (null != columnConstraintContext.primaryKeyConstraint() && null != columnConstraintContext.primaryKeyConstraint().primaryKey())
                {
                    return true;
                }
            }
            return false;
        }

        public override IASTNode VisitTableConstraint(SqlServerCommandParser.TableConstraintContext context)
        {
            ConstraintDefinitionSegment result = new ConstraintDefinitionSegment(context.Start.StartIndex, context.Stop.StopIndex);
            if (null != context.tablePrimaryConstraint() && null != context.tablePrimaryConstraint().primaryKeyUnique().primaryKey())
            {
                if (null != context.tablePrimaryConstraint().diskTablePrimaryConstraintOption())
                {
                    result.PrimaryKeyColumns.AddAll(((CollectionValue<ColumnSegment>)Visit(context.tablePrimaryConstraint().diskTablePrimaryConstraintOption().columnNames())).GetValue());
                }
                if (null != context.tablePrimaryConstraint().memoryTablePrimaryConstraintOption())
                {
                    result.PrimaryKeyColumns.AddAll(((CollectionValue<ColumnSegment>)Visit(context.tablePrimaryConstraint().memoryTablePrimaryConstraintOption().columnNames())).GetValue());
                }
            }
            if (null != context.tableForeignKeyConstraint())
            {
                result.ReferencedTable= (SimpleTableSegment)Visit(context.tableForeignKeyConstraint().tableName());
            }
            return result;
        }

        public override IASTNode VisitAlterTable(SqlServerCommandParser.AlterTableContext context)
        {
            AlterTableCommand result = new AlterTableCommand((SimpleTableSegment)Visit(context.tableName()));
            foreach (var alterDefinitionClauseContext in context.alterDefinitionClause())
            {
                foreach (var alterDefinitionSegment in ((CollectionValue<IAlterDefinitionSegment>)Visit(alterDefinitionClauseContext)).GetValue())
                {
                    if (alterDefinitionSegment is AddColumnDefinitionSegment addColumnDefinitionSegment) {
                        result.AddColumnDefinitions.Add(addColumnDefinitionSegment);
                    } else if (alterDefinitionSegment is ModifyColumnDefinitionSegment modifyColumnDefinitionSegment) {
                        result.ModifyColumnDefinitions.Add(modifyColumnDefinitionSegment);
                    } else if (alterDefinitionSegment is DropColumnDefinitionSegment dropColumnDefinitionSegment) {
                        result.DropColumnDefinitions.Add(dropColumnDefinitionSegment);
                    } else if (alterDefinitionSegment is ConstraintDefinitionSegment constraintDefinitionSegment) {
                        result.AddConstraintDefinitions.Add(constraintDefinitionSegment);
                    }
                }
            }
            return result;
        }

        public override IASTNode VisitAlterDefinitionClause(SqlServerCommandParser.AlterDefinitionClauseContext context)
        {
            CollectionValue<IAlterDefinitionSegment> result = new CollectionValue<IAlterDefinitionSegment>();
            if (null != context.addColumnSpecification())
            {
                var addColumnDefinitionSegments = ((CollectionValue<AddColumnDefinitionSegment>)Visit(context.addColumnSpecification())).GetValue();
                foreach (var addColumnDefinitionSegment in addColumnDefinitionSegments)
                {
                    result.GetValue().Add(addColumnDefinitionSegment);
                }
            }
            if (null != context.modifyColumnSpecification())
            {
                result.GetValue().Add((ModifyColumnDefinitionSegment)Visit(context.modifyColumnSpecification()));
            }
            if (null != context.alterDrop() && null != context.alterDrop().dropColumnSpecification())
            {
                result.GetValue().Add((DropColumnDefinitionSegment)Visit(context.alterDrop().dropColumnSpecification()));
            }
            return result;
        }

        public override IASTNode VisitAddColumnSpecification(SqlServerCommandParser.AddColumnSpecificationContext context)
        {
            CollectionValue<AddColumnDefinitionSegment> result = new CollectionValue<AddColumnDefinitionSegment>();
            if (null != context.alterColumnAddOptions())
            {
                foreach (var alterColumnAddOptionContext in context.alterColumnAddOptions().alterColumnAddOption())
                {
                    if (null != alterColumnAddOptionContext.columnDefinition())
                    {
                        AddColumnDefinitionSegment addColumnDefinition = new AddColumnDefinitionSegment(
                            alterColumnAddOptionContext.columnDefinition().Start.StartIndex, alterColumnAddOptionContext.columnDefinition().Stop.StopIndex,
                            new List<ColumnDefinitionSegment>(){ (ColumnDefinitionSegment)Visit(alterColumnAddOptionContext.columnDefinition()) });
                        result.GetValue().Add(addColumnDefinition);
                    }
                }
            }
            return result;
        }

        public override IASTNode VisitModifyColumnSpecification(SqlServerCommandParser.ModifyColumnSpecificationContext context)
        {
            // TODO visit pk and table ref
            ColumnSegment column = (ColumnSegment)Visit(context.alterColumnOperation().columnName());
            DataTypeSegment dataType = (DataTypeSegment)Visit(context.dataType());
            ColumnDefinitionSegment columnDefinition = new ColumnDefinitionSegment(context.Start.StartIndex, context.Stop.StopIndex, column, dataType, false);
            return new ModifyColumnDefinitionSegment(context.Start.StartIndex, context.Stop.StopIndex, columnDefinition);
        }

        public override IASTNode VisitDropColumnSpecification(SqlServerCommandParser.DropColumnSpecificationContext context)
        {
            ICollection<ColumnSegment> columns = new LinkedList<ColumnSegment>();
            foreach (var columnNameContext in context.columnName())
            {
                columns.Add((ColumnSegment)Visit(columnNameContext));
            }
            return new DropColumnDefinitionSegment(context.Start.StartIndex, context.Stop.StopIndex, columns);
        }

        public override IASTNode VisitDropTable(SqlServerCommandParser.DropTableContext context)
        {
            DropTableCommand result = new DropTableCommand();
            result.Tables.AddAll(((CollectionValue<SimpleTableSegment>)Visit(context.tableNames())).GetValue());
            return result;
        }

        public override IASTNode VisitTruncateTable(SqlServerCommandParser.TruncateTableContext context)
        {
            TruncateCommand result = new TruncateCommand();
            result.Tables.Add((SimpleTableSegment)Visit(context.tableName()));
            return result;
        }

        public override IASTNode VisitCreateIndex(SqlServerCommandParser.CreateIndexContext context)
        {
            CreateIndexCommand result = new CreateIndexCommand();
            result.Table = (SimpleTableSegment) Visit(context.tableName());
            result.Index= (IndexSegment)Visit(context.indexName());
            return result;
        }

        public override IASTNode VisitAlterIndex(SqlServerCommandParser.AlterIndexContext context)
        {
            AlterIndexCommand result = new AlterIndexCommand();
            if (null != context.indexName())
            {
                result.Index= (IndexSegment)Visit(context.indexName());
            }
            result.Table=(SimpleTableSegment)Visit(context.tableName());
            return result;
        }

        public override IASTNode VisitDropIndex(SqlServerCommandParser.DropIndexContext context)
        {
            DropIndexCommand result = new DropIndexCommand();
            result.Indexes.Add((IndexSegment)Visit(context.indexName()));
            result.Table= (SimpleTableSegment)Visit(context.tableName());
            return result;
        }
    }
}
