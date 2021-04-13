using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ShardingConnector.Parser.Binder.Segment.Insert;
using ShardingConnector.Parser.Binder.Segment.Insert.Values;
using ShardingConnector.Parser.Binder.Segment.Table;
using ShardingConnector.Parser.Sql.Command.DML;
using ShardingConnector.Parser.Sql.Segment.Generic.Table;

namespace ShardingConnector.Parser.Binder.Command.DML
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 8:01:58
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class InsertCommandContext:GenericSqlCommandContext<InsertCommand>,ITableAvailable
    {
        private readonly TablesContext _tablesContext;
    
    private readonly List<string> _columnNames;

        private readonly List<InsertValueContext> _insertValueContexts;

        private readonly GeneratedKeyContext _generatedKeyContext;
    
    public InsertCommandContext(final SchemaMetaData schemaMetaData, final List<Object> parameters, final InsertStatement sqlStatement)
        {
            super(sqlStatement);
            tablesContext = new TablesContext(sqlStatement.getTable());
            columnNames = sqlStatement.useDefaultColumns() ? schemaMetaData.getAllColumnNames(sqlStatement.getTable().getTableName().getIdentifier().getValue()) : sqlStatement.getColumnNames();
            insertValueContexts = getInsertValueContexts(parameters);
            generatedKeyContext = new GeneratedKeyContextEngine(schemaMetaData).createGenerateKeyContext(parameters, sqlStatement).orElse(null);
        }

        private List<InsertValueContext> getInsertValueContexts(final List<Object> parameters)
        {
            List<InsertValueContext> result = new LinkedList<>();
            int parametersOffset = 0;
            for (Collection<> ExpressionSegment> each : getSqlStatement().getAllValueExpressions())
            {
                InsertValueContext insertValueContext = new InsertValueContext(each, parameters, parametersOffset);
                result.add(insertValueContext);
                parametersOffset += insertValueContext.getParametersCount();
            }
            return result;
        }

        /**
         * Get column names for descending order.
         * 
         * @return column names for descending order
         */
        public Iterator<String> getDescendingColumnNames()
        {
            return new LinkedList<>(columnNames).descendingIterator();
        }

        /**
         * Get grouped parameters.
         * 
         * @return grouped parameters
         */
        public List<List<Object>> getGroupedParameters()
        {
            List<List<Object>> result = new LinkedList<>();
            for (InsertValueContext each : insertValueContexts)
            {
                result.add(each.getParameters());
            }
            return result;
        }

        /**
         * Get generated key context.
         * 
         * @return generated key context
         */
        public Optional<GeneratedKeyContext> getGeneratedKeyContext()
        {
            return Optional.ofNullable(generatedKeyContext);
        }

        @Override
    public Collection<SimpleTableSegment> getAllTables()
        {
            return Collections.singletonList(getSqlStatement().getTable());
        }
    }
}
