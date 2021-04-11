using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShardingConnector.Kernels.MetaData.Schema;
using ShardingConnector.Parser.Binder.Segment.Select.Groupby;
using ShardingConnector.Parser.Binder.Segment.Select.Groupby.Engine;
using ShardingConnector.Parser.Binder.Segment.Select.OrderBy;
using ShardingConnector.Parser.Binder.Segment.Select.OrderBy.Engine;
using ShardingConnector.Parser.Binder.Segment.Select.Pagination;
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
    
        private readonly ProjectionsContext _projectionsContext;
    
        private readonly GroupByContext _groupByContext;
    
        private readonly OrderByContext _orderByContext;
    
        private readonly PaginationContext _paginationContext;
    
        private readonly bool _containsSubQuery;

        // TODO to be remove, for test case only
        public SelectCommandContext(SelectCommand sqlCommand,GroupByContext groupByContext,
        OrderByContext orderByContext, ProjectionsContext projectionsContext, PaginationContext paginationContext):base(sqlCommand) {
            _tablesContext = new TablesContext(sqlCommand.GetSimpleTableSegments());
            this._groupByContext = _groupByContext;
            this._orderByContext = _orderByContext;
            this._projectionsContext = _projectionsContext;
            this._paginationContext = _paginationContext;
            _containsSubQuery = ContainsSubQuery();
        }
    
        public SelectCommandContext(SchemaMetaData schemaMetaData, string sql, List<Object> parameters,SelectCommand sqlCommand):base(sqlCommand) {
            _tablesContext = new TablesContext(sqlCommand.GetSimpleTableSegments());
            _groupByContext = GroupByContextEngine.CreateGroupByContext(sqlCommand);
            _orderByContext = OrderByContextEngine.CreateOrderBy(sqlCommand,_groupByContext);
            _projectionsContext = new ProjectionsContextEngine(schemaMetaData).createProjectionsContext(sql, sqlStatement, _groupByContext, _orderByContext);
            _paginationContext = new PaginationContextEngine().createPaginationContext(sqlStatement, _projectionsContext, parameters);
            _containsSubQuery = ContainsSubQuery();
        }

        public ICollection<SimpleTableSegment> GetAllTables()
        {
            throw new NotImplementedException();
        }

        public WhereSegment GetWhere()
        {
            return 
        }

        public ProjectionsContext GetProjectionsContext()
        {
            return _projectionsContext;
        }
        public GroupByContext GetGroupByContext()
        {
            return _groupByContext;
        }
        
        public bool IsSameGroupByAndOrderByItems() {
            return _groupByContext.GetItems().Any() && _groupByContext.GetItems().Equals(_orderByContext.GetItems());
        }
        
        private bool ContainsSubQuery() {
            // FIXME process subquery
//        Collection<SubqueryPredicateSegment> subqueryPredicateSegments = getSqlStatement().findSQLSegments(SubqueryPredicateSegment.class);
//        for (SubqueryPredicateSegment each : subqueryPredicateSegments) {
//            if (!each.getAndPredicates().isEmpty()) {
//                return true;
//            }
//        }
            return false;
        }
    }
}
