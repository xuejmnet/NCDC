using System.Collections.Generic;
using System.Linq;
using NCDC.CommandParser.Common.Segment.DML.Assignment;
using NCDC.CommandParser.Common.Segment.DML.Column;
using NCDC.CommandParser.Common.Segment.DML.Expr;
using NCDC.CommandParser.Common.Segment.DML.Expr.SubQuery;
using NCDC.CommandParser.Common.Segment.Generic.Table;

namespace NCDC.CommandParser.Common.Command.DML
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: Monday, 12 April 2021 22:38:40
    * @Email: 326308290@qq.com
    */
    public abstract class InsertCommand: AbstractSqlCommand, IDMLCommand
    {

        public SimpleTableSegment? Table { get; set; }

        public InsertColumnsSegment? InsertColumns { get; set; }
        public SubQuerySegment? InsertSelect { get; set; }
        public readonly ICollection<InsertValuesSegment> Values = new LinkedList<InsertValuesSegment>();
    }
}