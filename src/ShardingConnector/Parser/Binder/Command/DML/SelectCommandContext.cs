using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Parser.Binder.Segment.Select.Groupby;
using ShardingConnector.Parser.Binder.Segment.Select.OrderBy;
using ShardingConnector.Parser.Binder.Segment.Select.Projection;
using ShardingConnector.Parser.Binder.Segment.Table;
using ShardingConnector.Parser.Sql.Command.DML;
using ShardingConnector.Parser.Sql.Segment.Generic.Table;
using ShardingConnector.Parser.Sql.Segment.Predicate;

namespace ShardingConnector.Parser.Binder.Command.DML
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/10 9:43:59
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SelectCommandContext:GenericSqlCommandContext<SelectCommand>,ITableAvailable,IWhereAvailable
    {
        private readonly TablesContext _tablesContext;
    
        private readonly ProjectionsContext projectionsContext;
    
        private readonly GroupByContext groupByContext;
    
        private readonly OrderByContext orderByContext;
    
        private readonly PaginationContext paginationContext;
    
        private readonly bool containsSubquery;

        // TODO to be remove, for test case only
        public SelectStatementContext(final SelectStatement sqlStatement, final GroupByContext groupByContext,
        final OrderByContext orderByContext, final ProjectionsContext projectionsContext, final PaginationContext paginationContext) {
            super(sqlStatement);
            tablesContext = new TablesContext(sqlStatement.getSimpleTableSegments());
            this.groupByContext = groupByContext;
            this.orderByContext = orderByContext;
            this.projectionsContext = projectionsContext;
            this.paginationContext = paginationContext;
            containsSubquery = containsSubquery();
        }
    
        public SelectStatementContext(final SchemaMetaData schemaMetaData, final String sql, final List<Object> parameters, final SelectStatement sqlStatement) {
            super(sqlStatement);
            tablesContext = new TablesContext(sqlStatement.getSimpleTableSegments());
            groupByContext = new GroupByContextEngine().createGroupByContext(sqlStatement);
            orderByContext = new OrderByContextEngine().createOrderBy(sqlStatement, groupByContext);
            projectionsContext = new ProjectionsContextEngine(schemaMetaData).createProjectionsContext(sql, sqlStatement, groupByContext, orderByContext);
            paginationContext = new PaginationContextEngine().createPaginationContext(sqlStatement, projectionsContext, parameters);
            containsSubquery = containsSubquery();
        }

        public ICollection<SimpleTableSegment> GetAllTables()
        {
            throw new NotImplementedException();
        }

        public WhereSegment GetWhere()
        {
            throw new NotImplementedException();
        }

        public ProjectionsContext GetProjectionsContext()
        {
            return projectionsContext;
        }
        public GroupByContext GetGroupByContext()
        {
            return groupByContext;
        }
    }
}
