using NCDC.CommandParser.Common.Constant;
using NCDC.CommandParser.Common.Segment.DML.Item;
using NCDC.CommandParser.Common.Segment.Generic.Table;
using NCDC.ShardingParser.Segment.Select.Projection.Impl;
using NCDC.Basic.TableMetadataManagers;
using NCDC.CommandParser.Common.Segment.DML.Expr.Simple;
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
        public IProjection? CreateProjection(ITableSegment table, IProjectionSegment projectionSegment)
        {
            if (projectionSegment is ShorthandProjectionSegment shorthandProjectionSegment)
            {
                return CreateProjection(table, shorthandProjectionSegment);
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
                return CreateProjection(aggregationDistinctProjectionSegment);
            }

            if (projectionSegment is AggregationProjectionSegment aggregationProjectionSegment)
            {
                return CreateProjection(aggregationProjectionSegment);
            }

            if (projectionSegment is SubQueryProjectionSegment subQueryProjectionSegment)
            {
                return CreateProjection(subQueryProjectionSegment);
            }

            if (projectionSegment is ParameterMarkerExpressionSegment parameterMarkerExpressionSegment)
            {
                return CreateProjection(parameterMarkerExpressionSegment);
            }

            // TODO subquery
            return null;
        }

        private ParameterMarkerProjection CreateProjection(ParameterMarkerExpressionSegment projectionSegment)
        {
            return new ParameterMarkerProjection(projectionSegment.ParameterMarkerIndex,
                projectionSegment.ParameterMarkerType, projectionSegment.GetAlias());
        }

        private SubQueryProjection CreateProjection(SubQueryProjectionSegment projectionSegment)
        {
            return new SubQueryProjection(projectionSegment.Text, projectionSegment.GetAlias());
        }

        private ShorthandProjection CreateProjection(ITableSegment table, ShorthandProjectionSegment projectionSegment)
        {
            var owner = projectionSegment.Owner?.IdentifierValue.Value;
            var columnProjections = new LinkedList<ColumnProjection>();
            columnProjections.AddAll(GetShorthandColumnsFromSimpleTableSegment(table, owner));
            columnProjections.AddAll(GetShorthandColumnsFromSubQueryTableSegment(table));
            columnProjections.AddAll(GetShorthandColumnsFromJoinTableSegment(table, projectionSegment));
            return new ShorthandProjection(owner, columnProjections);
        }

        private ColumnProjection CreateProjection(ColumnProjectionSegment projectionSegment)
        {
            var owner = projectionSegment.Column.Owner?.IdentifierValue.Value;
            return new ColumnProjection(owner, projectionSegment.Column.IdentifierValue.Value,
                projectionSegment.GetAlias());
        }

        private ExpressionProjection CreateProjection(ExpressionProjectionSegment projectionSegment)
        {
            return new ExpressionProjection(projectionSegment.Text, projectionSegment.GetAlias());
        }

        private AggregationDistinctProjection CreateProjection(AggregationDistinctProjectionSegment projectionSegment)
        {
            var innerExpression = projectionSegment.InnerExpression;
            var alias = projectionSegment.GetAlias() ?? DerivedColumn
                .Get(DerivedColumnEnum.AGGREGATION_DISTINCT_DERIVED)
                .GetDerivedColumnAlias(aggregationDistinctDerivedColumnCount++);
            AggregationDistinctProjection result = new AggregationDistinctProjection(
                projectionSegment.StartIndex, projectionSegment.StopIndex, projectionSegment.Type, innerExpression,
                alias, projectionSegment.DistinctExpression);
            if (AggregationTypeEnum.AVG == result.GetAggregationType())
            {
                AppendAverageDistinctDerivedProjection(result);
            }

            return result;
        }

        private AggregationProjection CreateProjection(AggregationProjectionSegment projectionSegment)
        {
            var innerExpression = projectionSegment.InnerExpression;
            AggregationProjection result =
                new AggregationProjection(projectionSegment.Type, innerExpression, projectionSegment.GetAlias());
            if (AggregationTypeEnum.AVG == result.GetAggregationType())
            {
                AppendAverageDerivedProjection(result);
                // TODO replace avg to constant, avoid calculate useless avg
            }

            return result;
        }

        // private ICollection<ColumnProjection> GetShorthandColumns(ICollection<SimpleTableSegment> tables, string? owner)
        // {
        //     return null == owner ? GetUnqualifiedShorthandColumns(tables) : GetQualifiedShorthandColumns(tables, owner);
        // }

        // private ICollection<ColumnProjection> GetUnqualifiedShorthandColumns(ICollection<SimpleTableSegment> tables)
        // {
        //     List<ColumnProjection> result =
        //         tables.SelectMany(table =>
        //         {
        //             var tableName = table.GetTableName().GetIdentifier().GetValue();
        //             return _tableMetadataManager.GetAllColumnNames(tableName).Select(columnName => new ColumnProjection(null, columnName, null));
        //                 
        //         }).ToList();
        //     return result;
        // }

        // private ICollection<ColumnProjection> GetQualifiedShorthandColumns(ICollection<SimpleTableSegment> tables, string owner)
        // {
        //
        //     foreach (var table in tables)
        //     {
        //         string tableName = table.GetTableName().GetIdentifier().GetValue();
        //         if (owner.Equals(table.GetAlias() ?? tableName))
        //         {
        //             return _tableMetadataManager.GetAllColumnNames(tableName).Select(columnName => new ColumnProjection(owner, columnName, null)).ToList();
        //         }
        //     }
        //     return new List<ColumnProjection>(0);
        // }

        private void AppendAverageDistinctDerivedProjection(AggregationDistinctProjection averageDistinctProjection)
        {
            var innerExpression = averageDistinctProjection.GetInnerExpression();
            var distinctInnerExpression = averageDistinctProjection.GetDistinctInnerExpression();
            var countAlias = DerivedColumn.Get(DerivedColumnEnum.AVG_COUNT_ALIAS)
                .GetDerivedColumnAlias(aggregationAverageDerivedColumnCount);
            AggregationDistinctProjection countDistinctProjection = new AggregationDistinctProjection(0, 0,
                AggregationTypeEnum.COUNT, innerExpression, countAlias, distinctInnerExpression);
            var sumAlias = DerivedColumn.Get(DerivedColumnEnum.AVG_SUM_ALIAS)
                .GetDerivedColumnAlias(aggregationAverageDerivedColumnCount);
            AggregationDistinctProjection sumDistinctProjection = new AggregationDistinctProjection(0, 0,
                AggregationTypeEnum.SUM, innerExpression, sumAlias, distinctInnerExpression);
            averageDistinctProjection.GetDerivedAggregationProjections().Add(countDistinctProjection);
            averageDistinctProjection.GetDerivedAggregationProjections().Add(sumDistinctProjection);
            aggregationAverageDerivedColumnCount++;
        }

        private void AppendAverageDerivedProjection(AggregationProjection averageProjection)
        {
            String innerExpression = averageProjection.GetInnerExpression();
            String countAlias = DerivedColumn.Get(DerivedColumnEnum.AVG_COUNT_ALIAS)
                .GetDerivedColumnAlias(aggregationAverageDerivedColumnCount);
            AggregationProjection countProjection =
                new AggregationProjection(AggregationTypeEnum.COUNT, innerExpression, countAlias);
            String sumAlias = DerivedColumn.Get(DerivedColumnEnum.AVG_SUM_ALIAS)
                .GetDerivedColumnAlias(aggregationAverageDerivedColumnCount);
            AggregationProjection sumProjection =
                new AggregationProjection(AggregationTypeEnum.SUM, innerExpression, sumAlias);
            averageProjection.GetDerivedAggregationProjections().Add(countProjection);
            averageProjection.GetDerivedAggregationProjections().Add(sumProjection);
            aggregationAverageDerivedColumnCount++;
        }

        private IEnumerable<ColumnProjection> GetShorthandColumnsFromSimpleTableSegment(ITableSegment table,
            String? owner)
        {
            //todo 
            return Array.Empty<ColumnProjection>();
            // if (table is SimpleTableSegment simpleTableSegment) {
            //     var tableName = simpleTableSegment.TableName.IdentifierValue.Value;
            //     String tableAlias = table.GetAlias() ?? tableName;
            //     var schemaName = simpleTableSegment.Owner?.IdentifierValue.Value;
            //     if (null == owner) {
            //         _schemas.get(schemaName).getVisibleColumnNames(tableName).stream().map(each -> new ColumnProjection(tableAlias, each, null)).forEach(result::add);
            //     } else if (owner.equalsIgnoreCase(tableAlias)) {
            //         schemas.get(schemaName).getVisibleColumnNames(tableName).stream().map(each -> new ColumnProjection(owner, each, null)).forEach(result::add);
            //     }
            // }
        }

        private ICollection<ColumnProjection> GetShorthandColumnsFromSubQueryTableSegment(ITableSegment table)
        {
            if (!(table is SubQueryTableSegment subQueryTableSegment))
            {
                return Array.Empty<ColumnProjection>();
            }

            var subSelectCommand = subQueryTableSegment.SubQuery.Select;
            var projections = subSelectCommand.Projections
                .Projections.Select(o => CreateProjection(subSelectCommand.From, o)).Where(o => o is not null).ToList();

            return GetColumnProjections(projections);
        }

        private ICollection<ColumnProjection> GetColumnProjections(ICollection<IProjection> projections)
        {
            ICollection<ColumnProjection> result = new LinkedList<ColumnProjection>();
            foreach (var projection in projections)
            {
                if (projection is ColumnProjection columnProjection)
                {
                    result.Add(columnProjection);
                }

                if (projection is ShorthandProjection shorthandProjection)
                {
                    result.AddAll(shorthandProjection.GetActualColumns().Values);
                }
            }

            return result;
        }

        private ICollection<ColumnProjection> GetShorthandColumnsFromJoinTableSegment(ITableSegment table,
            IProjectionSegment projectionSegment)
        {
            if (!(table is JoinTableSegment joinTableSegment))
            {
                return Array.Empty<ColumnProjection>();
            }

            ICollection<IProjection> projections = new LinkedList<IProjection>();
            var leftProjection = CreateProjection(joinTableSegment.Left, projectionSegment);
            if (leftProjection is not null)
            {
                projections.Add(leftProjection);
            }

            var rightProjection = CreateProjection(joinTableSegment.Right, projectionSegment);
            if (rightProjection is not null)
            {
                projections.Add(rightProjection);
            }

            return GetColumnProjections(projections);
        }
    }
}