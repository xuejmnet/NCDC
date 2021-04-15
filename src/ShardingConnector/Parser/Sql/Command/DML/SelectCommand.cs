﻿using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Parser.Sql.Segment;
using ShardingConnector.Parser.Sql.Segment.DML.Item;
using ShardingConnector.Parser.Sql.Segment.DML.Order;
using ShardingConnector.Parser.Sql.Segment.DML.Pagination.limit;
using ShardingConnector.Parser.Sql.Segment.Generic.Table;
using ShardingConnector.Parser.Sql.Segment.Predicate;

namespace ShardingConnector.Parser.Sql.Command.DML
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/10 9:39:02
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SelectCommand : DMLCommand
    {
        public ProjectionsSegment Projections;
        /// <summary>
        /// 
        /// </summary>
        public readonly ICollection<TableReferenceSegment> TableReferences = new LinkedList<TableReferenceSegment>();
        public WhereSegment Where;
        public GroupBySegment GroupBy;
        public OrderBySegment OrderBy;
        public LimitSegment Limit;
        public SelectCommand ParentCommand;
        public LockSegment Lock;


        /// <summary>
        /// Get simple table segments.
        /// </summary>
        /// <returns></returns>
        public ICollection<SimpleTableSegment> GetSimpleTableSegments()
        {
            ICollection<SimpleTableSegment> result = new LinkedList<SimpleTableSegment>();
            foreach (var tableReference in TableReferences)
            {
                foreach (var simpleTableSegment in tableReference.GetTables())
                {
                    result.Add(simpleTableSegment);
                }
            }

            return result;
        }
    }
}