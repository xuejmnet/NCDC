﻿using NCDC.CommandParser.Command.DML;
using NCDC.CommandParser.Predicate;
using NCDC.CommandParser.Segment.DML.Predicate;
using NCDC.CommandParser.Segment.Generic.Table;
using NCDC.CommandParserBinder.Segment.Table;
using OpenConnector.Extensions;

namespace NCDC.CommandParserBinder.Command.DML
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
    
        public DeleteCommandContext(DeleteCommand sqlCommand) : base(sqlCommand)
        {
            _tablesContext = new TablesContext(sqlCommand.Tables);
        }

        public ICollection<SimpleTableSegment> GetAllTables()
        {
            ICollection<SimpleTableSegment> result = new LinkedList<SimpleTableSegment>(GetSqlCommand().Tables);
            if (GetSqlCommand().Tables!=null)
            {
                result.AddAll(GetAllTablesFromWhere(GetSqlCommand().Where));
            }
            return result;
        }
        private ICollection<SimpleTableSegment> GetAllTablesFromWhere( WhereSegment where)
        {
            ICollection<SimpleTableSegment> result = new LinkedList<SimpleTableSegment>();
            foreach (var andPredicate in where.GetAndPredicates())
            {
                foreach (var predicate in andPredicate.GetPredicates())
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