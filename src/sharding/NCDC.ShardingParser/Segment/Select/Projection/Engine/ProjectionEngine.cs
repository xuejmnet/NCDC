using NCDC.CommandParser.Common.Constant;
using NCDC.CommandParser.Common.Segment.DML.Item;
using NCDC.CommandParser.Common.Segment.Generic.Table;
using NCDC.ShardingParser.Segment.Select.Projection.Impl;
using NCDC.Basic.TableMetadataManagers;
using NCDC.Extensions;

namespace NCDC.ShardingParser.Segment.Select.Projection.Engine
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: Sunday, 11 April 2021 21:52:44
    * @Email: 326308290@qq.com
    */
    public sealed class ProjectionEngine
    {
        private readonly ITableMetadataManager _tableMetadataManager;


        private int aggregationAverageDerivedColumnCount;

        private int aggregationDistinctDerivedColumnCount;

        public ProjectionEngine(ITableMetadataManager tableMetadataManager)
        {
            _tableMetadataManager = tableMetadataManager;
        }

        /**
         * Create projection.
         * 
         * @param sql SQL
         * @param tableSegments table segments
         * @param projectionSegment projection segment
         * @return projection
         */
        public IProjection CreateProjection(string sql, ICollection<SimpleTableSegment> tableSegments, IProjectionSegment projectionSegment)
        {
            if (projectionSegment is ShorthandProjectionSegment shorthandProjectionSegment)
            {
                return CreateProjection(tableSegments, shorthandProjectionSegment);
            }
            if (projectionSegment is ColumnProjectionSegment columnProjectionSegment)
            {
                return CreateProjection(columnProjectionSegment);
            }
            if (projectionSegment is ExpressionProjectionSegment expressionProjectionSegment)
            {
                return CreateProjection(expressionProjectionSegment);
            }
            if (projectionSegment is AggregationDistinctProjectionSegment aggregationDistinctProjectionSegment)
            {
                return CreateProjection(sql, aggregationDistinctProjectionSegment);
            }
            if (projectionSegment is AggregationProjectionSegment aggregationProjectionSegment)
            {
                return CreateProjection(sql, aggregationProjectionSegment);
            }
            // TODO subquery
            return null;
        }

        private ShorthandProjection CreateProjection(ICollection<SimpleTableSegment> tableSegments, ShorthandProjectionSegment projectionSegment)
        {
            var owner = projectionSegment.GetOwner()?.GetIdentifier().GetValue();
            ICollection<ColumnProjection> columns = GetShorthandColumns(tableSegments, owner);
            return new ShorthandProjection(owner, columns);
        }

        private ColumnProjection CreateProjection(ColumnProjectionSegment projectionSegment)
        {
            String owner = projectionSegment.GetColumn().GetOwner()?.GetIdentifier().GetValue();
            return new ColumnProjection(owner, projectionSegment.GetColumn().GetIdentifier().GetValue(), projectionSegment.GetAlias());
        }

        private ExpressionProjection CreateProjection(ExpressionProjectionSegment projectionSegment)
        {
            return new ExpressionProjection(projectionSegment.GetText(), projectionSegment.GetAlias());
        }

        private AggregationDistinctProjection CreateProjection(string sql, AggregationDistinctProjectionSegment projectionSegment)
        {
            var innerExpression = sql.SubStringWithEndIndex(projectionSegment.GetInnerExpressionStartIndex(), projectionSegment.GetStopIndex() + 1);
            var alias = projectionSegment.GetAlias() ?? DerivedColumn.Get(DerivedColumnEnum.AGGREGATION_DISTINCT_DERIVED).GetDerivedColumnAlias(aggregationDistinctDerivedColumnCount++);
            AggregationDistinctProjection result = new AggregationDistinctProjection(
                    projectionSegment.GetStartIndex(), projectionSegment.GetStopIndex(), projectionSegment.GetAggregationType(), innerExpression, alias, projectionSegment.GetDistinctExpression());
            if (AggregationTypeEnum.AVG == result.GetAggregationType())
            {
                AppendAverageDistinctDerivedProjection(result);
            }
            return result;
        }

        private AggregationProjection CreateProjection(string sql, AggregationProjectionSegment projectionSegment)
        {
            var innerExpression = sql.SubStringWithEndIndex(projectionSegment.GetInnerExpressionStartIndex(), projectionSegment.GetStopIndex() + 1);
            AggregationProjection result = new AggregationProjection(projectionSegment.GetAggregationType(), innerExpression, projectionSegment.GetAlias());
            if (AggregationTypeEnum.AVG == result.GetAggregationType())
            {
                AppendAverageDerivedProjection(result);
                // TODO replace avg to constant, avoid calculate useless avg
            }
            return result;
        }

        private ICollection<ColumnProjection> GetShorthandColumns(ICollection<SimpleTableSegment> tables, string owner)
        {
            return null == owner ? GetUnqualifiedShorthandColumns(tables) : GetQualifiedShorthandColumns(tables, owner);
        }

        private ICollection<ColumnProjection> GetUnqualifiedShorthandColumns(ICollection<SimpleTableSegment> tables)
        {
            List<ColumnProjection> result =
                tables.SelectMany(table =>
                {
                    var tableName = table.GetTableName().GetIdentifier().GetValue();
                    return _tableMetadataManager.GetAllColumnNames(tableName).Select(columnName => new ColumnProjection(null, columnName, null));
                        
                }).ToList();
            return result;
        }

        private ICollection<ColumnProjection> GetQualifiedShorthandColumns(ICollection<SimpleTableSegment> tables, string owner)
        {

            foreach (var table in tables)
            {
                string tableName = table.GetTableName().GetIdentifier().GetValue();
                if (owner.Equals(table.GetAlias() ?? tableName))
                {
                    return _tableMetadataManager.GetAllColumnNames(tableName).Select(columnName => new ColumnProjection(owner, columnName, null)).ToList();
                }
            }
            return new List<ColumnProjection>(0);
        }

        private void AppendAverageDistinctDerivedProjection(AggregationDistinctProjection averageDistinctProjection)
        {
            var innerExpression = averageDistinctProjection.GetInnerExpression();
            var distinctInnerExpression = averageDistinctProjection.GetDistinctInnerExpression();
            var countAlias = DerivedColumn.Get(DerivedColumnEnum.AVG_COUNT_ALIAS).GetDerivedColumnAlias(aggregationAverageDerivedColumnCount);
            AggregationDistinctProjection countDistinctProjection = new AggregationDistinctProjection(0, 0, AggregationTypeEnum.COUNT, innerExpression, countAlias, distinctInnerExpression);
            var sumAlias = DerivedColumn.Get(DerivedColumnEnum.AVG_SUM_ALIAS).GetDerivedColumnAlias(aggregationAverageDerivedColumnCount);
            AggregationDistinctProjection sumDistinctProjection = new AggregationDistinctProjection(0, 0, AggregationTypeEnum.SUM, innerExpression, sumAlias, distinctInnerExpression);
            averageDistinctProjection.GetDerivedAggregationProjections().Add(countDistinctProjection);
            averageDistinctProjection.GetDerivedAggregationProjections().Add(sumDistinctProjection);
            aggregationAverageDerivedColumnCount++;
        }

        private void AppendAverageDerivedProjection(AggregationProjection averageProjection)
        {
            String innerExpression = averageProjection.GetInnerExpression();
            String countAlias = DerivedColumn.Get(DerivedColumnEnum.AVG_COUNT_ALIAS).GetDerivedColumnAlias(aggregationAverageDerivedColumnCount);
            AggregationProjection countProjection = new AggregationProjection(AggregationTypeEnum.COUNT, innerExpression, countAlias);
            String sumAlias = DerivedColumn.Get(DerivedColumnEnum.AVG_SUM_ALIAS).GetDerivedColumnAlias(aggregationAverageDerivedColumnCount);
            AggregationProjection sumProjection = new AggregationProjection(AggregationTypeEnum.SUM, innerExpression, sumAlias);
            averageProjection.GetDerivedAggregationProjections().Add(countProjection);
            averageProjection.GetDerivedAggregationProjections().Add(sumProjection);
            aggregationAverageDerivedColumnCount++;
        }
    }
}