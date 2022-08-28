using System;
using System.Collections.Generic;
using System.Linq;
using ShardingConnector.CommandParserBinder.Segment.Select.OrderBy;

namespace ShardingConnector.ShardingMerge.DQL.GroupBy
{
/*
* @Author: xjm
* @Description:
* @Date: Wednesday, 05 May 2021 21:32:53
* @Email: 326308290@qq.com
*/
    public class GroupByValue
    {
        private readonly List<object> _groupValues;

        public GroupByValue(IStreamDataReader streamDataReader, ICollection<OrderByItem> groupByItems)
        {
            _groupValues = GetGroupByValues(streamDataReader, groupByItems);
        }

        private List<object> GetGroupByValues(IStreamDataReader streamDataReader, ICollection<OrderByItem> groupByItems)
        {
            List<object> result = new List<object>(groupByItems.Count);
            foreach (var groupByItem in groupByItems)
            {
                result.Add(streamDataReader.GetValue(groupByItem.GetIndex()));
            }

            return result;
        }

        protected bool Equals(GroupByValue other)
        {
            return _groupValues.SequenceEqual(other._groupValues);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GroupByValue) obj);
        }

        public override int GetHashCode()
        {
            return (_groupValues != null ? _groupValues.Sum(o=>o.GetHashCode()).GetHashCode() : 0);
        }

        public List<object> GetGroupValues()
        {
            return _groupValues;
        }
    }
}