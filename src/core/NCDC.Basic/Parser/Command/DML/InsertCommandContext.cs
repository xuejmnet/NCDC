using NCDC.CommandParser.Command.DML;
using NCDC.CommandParser.Segment.Generic.Table;
using NCDC.Basic.Parser.MetaData;
using NCDC.Basic.Parser.MetaData.Schema;
using NCDC.Basic.Parser.Segment.Insert.Keygen;
using NCDC.Basic.Parser.Segment.Insert.Keygen.Engine;
using NCDC.Basic.Parser.Segment.Insert.Values;
using NCDC.Basic.Parser.Segment.Table;
using NCDC.Basic.TableMetadataManagers;
using NCDC.ShardingAdoNet;

namespace NCDC.Basic.Parser.Command.DML
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

        public InsertCommandContext(ITableMetadataManager tableMetadataManager, ParameterContext parameterContext, InsertCommand insertCommand) : base(insertCommand)
        {
            _tablesContext = new TablesContext(insertCommand.Table);
            _columnNames = insertCommand.UseDefaultColumns() ? tableMetadataManager.GetAllColumnNames(insertCommand.Table.GetTableName().GetIdentifier().GetValue()).ToList() : insertCommand.GetColumnNames();
            _insertValueContexts = GetInsertValueContexts(parameterContext);
            _generatedKeyContext = new GeneratedKeyContextEngine(tableMetadataManager).CreateGenerateKeyContext(parameterContext, insertCommand);
        }

        private List<InsertValueContext> GetInsertValueContexts(ParameterContext parameterContext)
        {
            List<InsertValueContext> result = new List<InsertValueContext>(GetSqlCommand().GetAllValueExpressions().Count);
            foreach (var valueExpression in GetSqlCommand().GetAllValueExpressions())
            {
                InsertValueContext insertValueContext = new InsertValueContext(valueExpression, parameterContext);
                result.Add(insertValueContext);
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
        public List<ParameterContext> GetGroupedParameters()
        {
            List<ParameterContext> result = new List<ParameterContext>();
            foreach (var insertValueContext in _insertValueContexts)
            {
                result.Add(insertValueContext.GetParameterContext());
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
