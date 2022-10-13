using System.Collections.Generic;
using NCDC.CommandParser.Common.Segment;
using NCDC.CommandParser.Common.Segment.DML;
using NCDC.CommandParser.Common.Segment.DML.Combine;
using NCDC.CommandParser.Common.Segment.DML.Item;
using NCDC.CommandParser.Common.Segment.DML.Order;
using NCDC.CommandParser.Common.Segment.DML.Pagination.limit;
using NCDC.CommandParser.Common.Segment.DML.Predicate;
using NCDC.CommandParser.Common.Segment.Generic.Table;

namespace NCDC.CommandParser.Common.Command.DML
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/10 9:39:02
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public abstract class SelectCommand: AbstractSqlCommand, IDMLCommand
    {
        public ProjectionsSegment? Projections { get; set; }
        public ITableSegment? From { get; set; }
        public WhereSegment? Where { get; set; }
        public GroupBySegment? GroupBy { get; set; }
        public HavingSegment? Having { get; set; }
        public OrderBySegment? OrderBy { get; set; }
        public CombineSegment? Combine { get; set; }
    }
}