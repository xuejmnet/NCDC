using NCDC.CommandParser.Command.DML;
using NCDC.CommandParser.Predicate;
using NCDC.CommandParser.Segment.DML.Predicate;
using NCDC.CommandParser.Segment.Generic.Table;
using NCDC.Basic.Parser.Segment.Table;
using OpenConnector.Extensions;

namespace NCDC.Basic.Parser.Command.DML
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
    
        public UpdateCommandContext(UpdateCommand sqlCommand) : base(sqlCommand)
        {
            _tablesContext = new TablesContext(sqlCommand.Tables);
        }

        public ICollection<SimpleTableSegment> GetAllTables()
        {
            ICollection<SimpleTableSegment> result = new LinkedList<SimpleTableSegment>(GetSqlCommand().Tables);
            if (GetSqlCommand().Where!=null)
            {
                result.AddAll(GetAllTablesFromWhere(GetSqlCommand().Where));
            }
            return result;
        }
        private ICollection<SimpleTableSegment> GetAllTablesFromWhere(WhereSegment where)
        {
            ICollection<SimpleTableSegment> result = new LinkedList<SimpleTableSegment>();
            foreach (var andPredicateSegment in where.GetAndPredicates())
            {
                foreach (var predicate in andPredicateSegment.GetPredicates())
                {
                    result.AddAll(new PredicateExtractor(GetSqlCommand().Tables, predicate).ExtractTables());
                }
            }
            return result;
        }

        public WhereSegment GetWhere()
        {
            return GetSqlCommand().Where;
        }
        public override TablesContext GetTablesContext()
        {
            return _tablesContext;
        }
    }
}
