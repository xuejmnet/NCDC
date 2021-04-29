using System.Collections.Generic;
using ShardingConnector.CommandParser.Command.DML;
using ShardingConnector.CommandParser.Segment.Generic.Table;
using ShardingConnector.ParserBinder.MetaData.Schema;
using ShardingConnector.ParserBinder.Segment.Insert.Keygen;
using ShardingConnector.ParserBinder.Segment.Insert.Keygen.Engine;
using ShardingConnector.ParserBinder.Segment.Insert.Values;
using ShardingConnector.ParserBinder.Segment.Table;

namespace ShardingConnector.ParserBinder.Command.DML
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 8:01:58
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class InsertCommandContext : GenericSqlCommandContext<InsertCommand>, ITableAvailable
    {
        private readonly TablesContext _tablesContext;

        private readonly List<string> _columnNames;

        private readonly List<InsertValueContext> _insertValueContexts;

        private readonly GeneratedKeyContext _generatedKeyContext;

        public InsertCommandContext(SchemaMetaData schemaMetaData, List<object> parameters, InsertCommand insertCommand) : base(insertCommand)
        {
            _tablesContext = new TablesContext(insertCommand.Table);
            _columnNames = insertCommand.UseDefaultColumns() ? schemaMetaData.GetAllColumnNames(insertCommand.Table.GetTableName().GetIdentifier().GetValue()) : insertCommand.GetColumnNames();
            _insertValueContexts = GetInsertValueContexts(parameters);
            _generatedKeyContext = new GeneratedKeyContextEngine(schemaMetaData).CreateGenerateKeyContext(parameters, insertCommand);
        }

        private List<InsertValueContext> GetInsertValueContexts(List<object> parameters)
        {
            List<InsertValueContext> result = new List<InsertValueContext>(GetSqlCommand().GetAllValueExpressions().Count);
            int parametersOffset = 0;
            foreach (var valueExpression in GetSqlCommand().GetAllValueExpressions())
            {
                InsertValueContext insertValueContext = new InsertValueContext(valueExpression, parameters, parametersOffset);
                result.Add(insertValueContext);
                parametersOffset += insertValueContext.GetParametersCount();
            }
            return result;
        }

        /**
         * Get column names for descending order.
         * 
         * @return column names for descending order
         */
        public List<string> GetDescendingColumnNames()
        {
            var list = new List<string>(_columnNames);
            list.Reverse();
            return list;
        }

        /**
         * Get grouped parameters.
         * 
         * @return grouped parameters
         */
        public List<List<object>> GetGroupedParameters()
        {
            List<List<object>> result = new List<List<object>>();
            foreach (var insertValueContext in _insertValueContexts)
            {
                result.Add(insertValueContext.GetParameters());
            }
            return result;
        }

        /**
         * Get generated key context.
         * 
         * @return generated key context
         */
        public GeneratedKeyContext GetGeneratedKeyContext()
        {
            return _generatedKeyContext;
        }

        public ICollection<SimpleTableSegment> GetAllTables()
        {
            return new List<SimpleTableSegment>() { GetSqlCommand().Table };
        }

        public List<InsertValueContext> GetInsertValueContexts()
        {
            return _insertValueContexts;
        }

        public List<string> GetColumnNames()
        {
            return _columnNames;
        }
        public override TablesContext GetTablesContext()
        {
            return _tablesContext;
        }
    }
}
