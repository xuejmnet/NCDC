using System;
using System.Collections.Generic;
using System.Linq;
using ShardingConnector.Kernels.MetaData.Schema;
using ShardingConnector.Parser.Binder.Segment.Select.Projection.Impl;
using ShardingConnector.Parser.Sql.Constant;
using ShardingConnector.Parser.Sql.Segment.DML.Item;
using ShardingConnector.Parser.Sql.Segment.Generic.Table;

namespace ShardingConnector.Parser.Binder.Segment.Select.Projection.Engine
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 21:52:44
* @Email: 326308290@qq.com
*/
    public sealed class ProjectionEngine
    {
        
    private readonly SchemaMetaData _schemaMetaData;
    
    private int aggregationAverageDerivedColumnCount;
    
    private int aggregationDistinctDerivedColumnCount;

    public ProjectionEngine(SchemaMetaData schemaMetaData)
    {
        _schemaMetaData = schemaMetaData;
    }

    /**
     * Create projection.
     * 
     * @param sql SQL
     * @param tableSegments table segments
     * @param projectionSegment projection segment
     * @return projection
     */
    public IProjection CreateProjection(string sql, ICollection<SimpleTableSegment> tableSegments, IProjectionSegment projectionSegment) {
        if (projectionSegment is ShorthandProjectionSegment) {
            return Optional.of(createProjection(tableSegments, (ShorthandProjectionSegment) projectionSegment));
        }
        if (projectionSegment instanceof ColumnProjectionSegment) {
            return Optional.of(createProjection((ColumnProjectionSegment) projectionSegment));
        }
        if (projectionSegment instanceof ExpressionProjectionSegment) {
            return Optional.of(createProjection((ExpressionProjectionSegment) projectionSegment));
        }
        if (projectionSegment instanceof AggregationDistinctProjectionSegment) {
            return Optional.of(createProjection(sql, (AggregationDistinctProjectionSegment) projectionSegment));
        }
        if (projectionSegment instanceof AggregationProjectionSegment) {
            return Optional.of(createProjection(sql, (AggregationProjectionSegment) projectionSegment));
        }
        // TODO subquery
        return Optional.empty();
    }
    
    private ShorthandProjection CreateProjection(ICollection<SimpleTableSegment> tableSegments, ShorthandProjectionSegment projectionSegment)
    {
        var owner = projectionSegment.GetOwner()?.GetIdentifier().GetValue();
        ICollection<ColumnProjection> columns = getShorthandColumns(tableSegments, owner);
        return new ShorthandProjection(owner, columns);
    }
    
    private ColumnProjection CreateProjection(ColumnProjectionSegment projectionSegment) {
        String owner = projectionSegment.GetColumn().GetOwner()?.GetIdentifier().GetValue();
        return new ColumnProjection(owner, projectionSegment.GetColumn().GetIdentifier().GetValue(), projectionSegment.GetAlias());
    }
    
    private ExpressionProjection CreateProjection(final ExpressionProjectionSegment projectionSegment) {
        return new ExpressionProjection(projectionSegment.getText(), projectionSegment.getAlias().orElse(null));
    }
    
    private AggregationDistinctProjection CreateProjection(final String sql, final AggregationDistinctProjectionSegment projectionSegment) {
        String innerExpression = sql.substring(projectionSegment.getInnerExpressionStartIndex(), projectionSegment.getStopIndex() + 1);
        String alias = projectionSegment.getAlias().orElse(DerivedColumn.AGGREGATION_DISTINCT_DERIVED.getDerivedColumnAlias(aggregationDistinctDerivedColumnCount++));
        AggregationDistinctProjection result = new AggregationDistinctProjection(
                projectionSegment.getStartIndex(), projectionSegment.getStopIndex(), projectionSegment.getType(), innerExpression, alias, projectionSegment.getDistinctExpression());
        if (AggregationType.AVG == result.getType()) {
            appendAverageDistinctDerivedProjection(result);
        }
        return result;
    }
    
    private AggregationProjection CreateProjection(final String sql, final AggregationProjectionSegment projectionSegment) {
        String innerExpression = sql.substring(projectionSegment.getInnerExpressionStartIndex(), projectionSegment.getStopIndex() + 1);
        AggregationProjection result = new AggregationProjection(projectionSegment.getType(), innerExpression, projectionSegment.getAlias().orElse(null));
        if (AggregationType.AVG == result.getType()) {
            appendAverageDerivedProjection(result);
            // TODO replace avg to constant, avoid calculate useless avg
        }
        return result;
    }
    
    private ICollection<ColumnProjection> GetShorthandColumns(ICollection<SimpleTableSegment> tables, string owner) {
        return null == owner ? GetUnqualifiedShorthandColumns(tables) : GetQualifiedShorthandColumns(tables, owner);
    }
    
    private ICollection<ColumnProjection> GetUnqualifiedShorthandColumns(ICollection<SimpleTableSegment> tables)
    {
        List<ColumnProjection> result =
            tables.SelectMany(table =>
            {
                return _schemaMetaData.GetAllColumnNames(table.GetTableName().GetIdentifier().GetValue())
                    .Select(columnName => new ColumnProjection(null, columnName, null)).ToList();
            }).ToList();
        return result;
    }
    
    private ICollection<ColumnProjection> GetQualifiedShorthandColumns(ICollection<SimpleTableSegment> tables, string owner) {
       
        foreach (var table in tables)
        {
            string tableName = table.GetTableName().GetIdentifier().GetValue();
            if (owner.Equals(table.GetAlias()??tableName)) {
                return _schemaMetaData.GetAllColumnNames(tableName).Select(columnName => new ColumnProjection(owner, columnName, null)).ToList();
            }
        }
        return new List<ColumnProjection>(0);
    }
    
    private void AppendAverageDistinctDerivedProjection(AggregationDistinctProjection averageDistinctProjection) {
        string innerExpression = averageDistinctProjection.GetInnerExpression();
        string distinctInnerExpression = averageDistinctProjection.GetDistinctInnerExpression();
        string countAlias = DerivedColumn.AVG_COUNT_ALIAS.getDerivedColumnAlias(_aggregationAverageDerivedColumnCount);
        AggregationDistinctProjection countDistinctProjection = new AggregationDistinctProjection(0, 0, AggregationType.COUNT, innerExpression, countAlias, distinctInnerExpression);
        string sumAlias = DerivedColumn.AVG_SUM_ALIAS.getDerivedColumnAlias(_aggregationAverageDerivedColumnCount);
        AggregationDistinctProjection sumDistinctProjection = new AggregationDistinctProjection(0, 0, AggregationType.SUM, innerExpression, sumAlias, distinctInnerExpression);
        averageDistinctProjection.getDerivedAggregationProjections().add(countDistinctProjection);
        averageDistinctProjection.getDerivedAggregationProjections().add(sumDistinctProjection);
        _aggregationAverageDerivedColumnCount++;
    }
    
    private void appendAverageDerivedProjection(final AggregationProjection averageProjection) {
        String innerExpression = averageProjection.getInnerExpression();
        String countAlias = DerivedColumn.AVG_COUNT_ALIAS.getDerivedColumnAlias(_aggregationAverageDerivedColumnCount);
        AggregationProjection countProjection = new AggregationProjection(AggregationType.COUNT, innerExpression, countAlias);
        String sumAlias = DerivedColumn.AVG_SUM_ALIAS.getDerivedColumnAlias(_aggregationAverageDerivedColumnCount);
        AggregationProjection sumProjection = new AggregationProjection(AggregationType.SUM, innerExpression, sumAlias);
        averageProjection.getDerivedAggregationProjections().add(countProjection);
        averageProjection.getDerivedAggregationProjections().add(sumProjection);
        _aggregationAverageDerivedColumnCount++;
    }
    }
}