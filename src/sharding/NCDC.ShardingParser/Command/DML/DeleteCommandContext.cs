using NCDC.CommandParser.Common.Command.DML;
using NCDC.CommandParser.Common.Extractors;
using NCDC.CommandParser.Common.Segment.DML.Column;
using NCDC.CommandParser.Common.Segment.DML.Predicate;
using NCDC.CommandParser.Common.Segment.Generic.Table;
using NCDC.CommandParser.Common.Util;
using NCDC.Extensions;
using NCDC.ShardingParser.Segment.Table;

namespace NCDC.ShardingParser.Command.DML
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/29 12:30:46
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class DeleteCommandContext: GenericSqlCommandContext<DeleteCommand>, ITableAvailable, IWhereAvailable
    {
        private readonly TablesContext _tablesContext;
        private readonly ICollection<WhereSegment> _whereSegments = new LinkedList<WhereSegment>();
        private readonly ICollection<ColumnSegment> _columnSegments=new LinkedList<ColumnSegment>();
    
        public DeleteCommandContext(DeleteCommand sqlCommand) : base(sqlCommand)
        {
            _tablesContext = new TablesContext(GetAllSimpleTableSegments());
            var whereSegment = GetSqlCommand().Where;
            if (whereSegment is not null)
            {
                _whereSegments.Add(whereSegment);
            }
            ColumnExtractor.ExtractColumnSegments(_columnSegments,_whereSegments);
        }

        private ICollection<SimpleTableSegment> GetAllSimpleTableSegments() {
            TableExtractor tableExtractor = new TableExtractor();
            tableExtractor.ExtractTablesFromDelete(GetSqlCommand());
            return FilterAliasDeleteTable(tableExtractor.RewriteTables);
        }
        private ICollection<SimpleTableSegment> FilterAliasDeleteTable(ICollection<SimpleTableSegment> tableSegments) {
            var aliasTableSegmentMap = new Dictionary<string,SimpleTableSegment>(tableSegments.Count);
            foreach (var simpleTableSegment in tableSegments)
            {
                var alias = simpleTableSegment.GetAlias();
                if (alias is not null)
                {
                    aliasTableSegmentMap.TryAdd(alias, simpleTableSegment);
                }
            }
            ICollection<SimpleTableSegment> result = new LinkedList<SimpleTableSegment>();
            foreach (var simpleTableSegment in tableSegments)
            {
                var tableName = simpleTableSegment.TableName.IdentifierValue.Value;
                if (aliasTableSegmentMap.TryGetValue(tableName, out var aliasDeleteTable)&&aliasDeleteTable.Equals(simpleTableSegment))
                {
                    result.Add(simpleTableSegment);
                }
            }
            return result;
        }

        public ICollection<SimpleTableSegment> GetAllTables()
        {
            return _tablesContext.GetTables();
        }
        public WhereSegment? GetWhere()
        {
            return GetSqlCommand().Where;
        }
        public override TablesContext GetTablesContext()
        {
            return _tablesContext;
        }
    }
}
