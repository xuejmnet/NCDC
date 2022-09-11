using NCDC.Basic.Parser.Segment.Select.Projection.Impl;

namespace NCDC.Basic.Parser.Segment.Select.Projection
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
            int result = 0;
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
            List<AggregationProjection> result = new List<AggregationProjection>();
            foreach (var projection in _projections)
            {
                if (projection is AggregationProjection aggregationProjection)
                {
                    result.Add(aggregationProjection);
                    result.AddRange(aggregationProjection.GetDerivedAggregationProjections());
                }
            }

            return result;
        }

       /// <summary>
       /// Get aggregation distinct projections.
       /// </summary>
       /// <returns></returns>
        public List<AggregationDistinctProjection> GetAggregationDistinctProjections()
        {
            List<AggregationDistinctProjection> result = new List<AggregationDistinctProjection>();
            foreach (var projection in _projections)
            {
                if (projection is AggregationDistinctProjection aggregationDistinctProjection)
                {
                    result.Add(aggregationDistinctProjection);
                }
            }

            return result;
        }

       /// <summary>
       /// Get expand projections with shorthand projections.
       /// </summary>
       /// <returns></returns>
        public List<IProjection> GetExpandProjections()
        {
            List<IProjection> result = new List<IProjection>();
            foreach (var projection in _projections)
            {
                if (projection is ShorthandProjection shorthandProjection)
                {
                    result.AddRange(shorthandProjection.GetActualColumns());
                }
                else if (!(projection is DerivedProjection))
                {
                    result.Add(projection);
                }
            }

            return result;
        }
    }
}