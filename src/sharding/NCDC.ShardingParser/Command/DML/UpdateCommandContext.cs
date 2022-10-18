using NCDC.CommandParser.Common.Command.DML;
using NCDC.CommandParser.Common.Extractors;
using NCDC.CommandParser.Common.Segment.DML.Column;
using NCDC.CommandParser.Common.Segment.DML.Predicate;
using NCDC.CommandParser.Common.Segment.Generic.Table;
using NCDC.CommandParser.Common.Util;
using NCDC.ShardingParser.Segment.Table;

namespace NCDC.ShardingParser.Command.DML
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/29 12:15:51
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class UpdateCommandContext: GenericSqlCommandContext<UpdateCommand>, ITableAvailable, IWhereAvailable
    {
        private readonly TablesContext _tablesContext;
        private readonly ICollection<WhereSegment> _whereSegments=new LinkedList<WhereSegment>();
        private readonly ICollection<ColumnSegment> _columnSegments=new LinkedList<ColumnSegment>();
    
        public UpdateCommandContext(UpdateCommand sqlCommand) : base(sqlCommand)
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
            tableExtractor.ExtractTablesFromUpdate(GetSqlCommand());
            return tableExtractor.RewriteTables;
        }

        public ICollection<SimpleTableSegment> GetAllTables()
        {
            return _tablesContext.GetTables();
        }
        // private ICollection<SimpleTableSegment> GetAllTablesFromWhere(WhereSegment where)
        // {
        //     ICollection<SimpleTableSegment> result = new LinkedList<SimpleTableSegment>();
        //     foreach (var andPredicateSegment in where.GetAndPredicates())
        //     {
        //         foreach (var predicate in andPredicateSegment.GetPredicates())
        //         {
        //             result.AddAll(new PredicateExtractor(GetSqlCommand().Tables, predicate).ExtractTables());
        //         }
        //     }
        //     return result;
        // }

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
