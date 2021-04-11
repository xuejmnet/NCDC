using System;
using System.Collections.Generic;
using System.Linq;
using ShardingConnector.Parser.Binder.Segment.Select.Projection.Impl;

namespace ShardingConnector.Parser.Binder.Segment.Select.Projection
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 14:55:37
* @Email: 326308290@qq.com
*/
    public sealed class ProjectionsContext
    {
        private readonly int _startIndex;

        private readonly int _stopIndex;

        private readonly bool _distinctRow;

        private readonly ICollection<IProjection> _projections;

        public ProjectionsContext(int startIndex, int stopIndex, bool distinctRow, ICollection<IProjection> projections)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            _distinctRow = distinctRow;
            _projections = projections;
        }


        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public bool IsDistinctRow()
        {
            return _distinctRow;
        }

        public ICollection<IProjection> GetProjections()
        {
            return _projections;
        }

        /// <summary>
        /// Judge is unqualified shorthand projection or not.
        /// </summary>
        /// <returns></returns>
        public bool IsUnqualifiedShorthandProjection()
        {
            if (1 != _projections.Count)
            {
                return false;
            }

            IProjection projection = _projections.First();
            return projection is ShorthandProjection shorthandProjection && shorthandProjection.GetOwner() == null;
        }

        /// <summary>
        /// Find alias.
        /// </summary>
        /// <param name="projectionName"></param>
        /// <returns></returns>
        public string FindAlias(string projectionName)
        {
            foreach (var projection in _projections)
            {
                if (projectionName.Equals(projection.GetExpression(), StringComparison.OrdinalIgnoreCase))
                    return projection.GetAlias();
            }

            return null;
        }

        /// <summary>
        /// Find projection index.
        /// </summary>
        /// <param name="projectionName"></param>
        /// <returns></returns>
        public int? FindProjectionIndex(string projectionName)
        {
            int result = 1;
            foreach (var projection in _projections)
            {
                if (projectionName.Equals(projection.GetExpression(), StringComparison.OrdinalIgnoreCase))
                    return result;
                result++;
            }

            return null;
        }

        /// <summary>
        /// Get aggregation projections.
        /// </summary>
        /// <returns></returns>
        public List<AggregationProjection> GetAggregationProjections()
        {
            List<AggregationProjection> result = new LinkedList<>();
            for (Projection each :
            projections) {
                if (each instanceof AggregationProjection) {
                    AggregationProjection aggregationProjection = (AggregationProjection) each;
                    result.add(aggregationProjection);
                    result.addAll(aggregationProjection.getDerivedAggregationProjections());
                }
            }
            return result;
        }

        /**
     * Get aggregation distinct projections.
     *
     * @return aggregation distinct projections
     */
        public List<AggregationDistinctProjection> getAggregationDistinctProjections()
        {
            List<AggregationDistinctProjection> result = new LinkedList<>();
            for (Projection each :
            projections) {
                if (each instanceof AggregationDistinctProjection) {
                    result.add((AggregationDistinctProjection) each);
                }
            }
            return result;
        }

        /**
     * Get expand projections with shorthand projections.
     * 
     * @return expand projections
     */
        public List<Projection> getExpandProjections()
        {
            List<Projection> result = new ArrayList<>();
            for (Projection each :
            projections) {
                if (each instanceof ShorthandProjection) {
                    result.addAll(((ShorthandProjection) each).getActualColumns());
                } else if (!(each instanceof DerivedProjection)) {
                    result.add(each);
                }
            }
            return result;
        }
    }
}