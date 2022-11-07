using NCDC.CommandParser.Common.Command.DML;
using NCDC.CommandParser.Common.Segment.Generic.Table;
using NCDC.ShardingParser.Segment.Insert.Keygen;
using NCDC.ShardingParser.Segment.Insert.Keygen.Engine;
using NCDC.ShardingParser.Segment.Insert.Values;
using NCDC.ShardingParser.Segment.Table;
using NCDC.CommandParser.Common.Extractors;
using NCDC.CommandParser.Common.Segment.DML.Assignment;
using NCDC.CommandParser.Common.Segment.DML.Column;
using NCDC.CommandParser.Common.Segment.DML.Expr;
using NCDC.CommandParser.Dialect.Handler.DML;
using NCDC.Extensions;
using NCDC.ShardingAdoNet;
using NCDC.ShardingParser.MetaData;

namespace NCDC.ShardingParser.Command.DML
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

        private readonly IReadOnlyList<string> _columnNames;
        private readonly List<string> _insertColumnNames;
        private readonly List<List<IExpressionSegment>> _valueExpressions;

        private readonly List<InsertValueContext> _insertValueContexts;
        private InsertSelectContext? _insertSelectContext;
        private OnDuplicateUpdateContext? _onDuplicateKeyUpdateValueContext;

        private  GeneratedKeyContext? _generatedKeyContext;

        public InsertCommandContext(ITableMetadataManager tableMetadataManager, ParameterContext parameterContext, InsertCommand insertCommand) : base(insertCommand)
        {
            _insertColumnNames = GetInsertColumnNames();
           _valueExpressions= GetAllValueExpressions(insertCommand);
           _insertValueContexts = GetInsertValueContexts(parameterContext, _valueExpressions);
           _insertSelectContext=GetInsertSelectContext(tableMetadataManager, parameterContext);
           _onDuplicateKeyUpdateValueContext = GetOnDuplicateKeyUpdateValueContext(parameterContext);
            _tablesContext = new TablesContext(GetAllSimpleTableSegments());
            var tableName = GetSqlCommand()?.Table?.TableName.IdentifierValue.Value;
            _columnNames = ContainsInsertColumns() ? _insertColumnNames :(tableName is not null? tableMetadataManager.GetAllColumnNames(tableName):new List<string>());
            _generatedKeyContext = new GeneratedKeyContextEngine(insertCommand,tableMetadataManager).CreateGenerateKeyContext(_insertColumnNames,_valueExpressions,parameterContext);
        }
        public List<String> GetInsertColumnNames()
        {
            InsertCommand insertCommand = GetSqlCommand();
            var setAssignmentSegment = InsertCommandHandler.GetSetAssignmentSegment(insertCommand);
            if (setAssignmentSegment is not null)
            {
                return setAssignmentSegment.Assignments.Select(o => o.GetColumns()[0].IdentifierValue.Value.ToLower()).ToList();
            }
            return insertCommand.GetColumns().Select(o => o.IdentifierValue.Value.ToLower()).ToList();
        }
        private List<List<IExpressionSegment>> GetAllValueExpressions( InsertCommand insertCommand) {
            var setAssignmentSegment = InsertCommandHandler.GetSetAssignmentSegment(insertCommand);
            if (setAssignmentSegment is not null)
            {
                return new List<List<IExpressionSegment>>()
                {
                    GetAllValueExpressionsFromSetAssignment(setAssignmentSegment)
                };
            }
            else
            {
                return GetAllValueExpressionsFromValues(insertCommand.Values);
            }}
        private List<IExpressionSegment> GetAllValueExpressionsFromSetAssignment( SetAssignmentSegment setAssignment)
        {
            return setAssignment.Assignments.Select(o => o.GetValue()).ToList();
        }
        private List<List<IExpressionSegment>> GetAllValueExpressionsFromValues(ICollection<InsertValuesSegment> values)
        {
            return values.Select(o => o.Values).ToList();
        }

        private List<InsertValueContext> GetInsertValueContexts(ParameterContext parameterContext,  List<List<IExpressionSegment>> valueExpressions)
        {
            return valueExpressions.Select(o => new InsertValueContext(o, parameterContext)).ToList();
        }
        private InsertSelectContext? GetInsertSelectContext(ITableMetadataManager tableMetadataManager, ParameterContext parameterContext) {
            var insertCommand = GetSqlCommand();
            var insertSelectSegment = insertCommand.InsertSelect;
            if (insertSelectSegment is  null)
            {
                return null;
            }
            
            var selectCommandContext = new SelectCommandContext(tableMetadataManager, parameterContext, insertSelectSegment.Select);
            InsertSelectContext insertSelectContext = new InsertSelectContext(selectCommandContext, parameterContext);
            return insertSelectContext;
        }
        
        private OnDuplicateUpdateContext? GetOnDuplicateKeyUpdateValueContext(ParameterContext parameterContext) {
            var onDuplicateKeyColumnsSegment = InsertCommandHandler.GetOnDuplicateKeyColumnsSegment(GetSqlCommand());
            if (onDuplicateKeyColumnsSegment is null) {
                return null;
            }

            var onDuplicateKeyColumns = onDuplicateKeyColumnsSegment.Columns;
            OnDuplicateUpdateContext onDuplicateUpdateContext = new OnDuplicateUpdateContext(onDuplicateKeyColumns,parameterContext);
            return onDuplicateUpdateContext;
        }
        private ICollection<SimpleTableSegment> GetAllSimpleTableSegments() {
            TableExtractor tableExtractor = new TableExtractor();
            tableExtractor.ExtractTablesFromInsert(GetSqlCommand());
            return tableExtractor.RewriteTables;
        }
        public bool ContainsInsertColumns() {
            InsertCommand insertCommand = GetSqlCommand();
            return insertCommand.GetColumns().IsNotEmpty() || InsertCommandHandler.GetSetAssignmentSegment(insertCommand) is not null;
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
        public GeneratedKeyContext? GetGeneratedKeyContext()
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

        public IReadOnlyList<string> GetColumnNames()
        {
            return _columnNames;
        }
        public override TablesContext GetTablesContext()
        {
            return _tablesContext;
        }
    }
}
