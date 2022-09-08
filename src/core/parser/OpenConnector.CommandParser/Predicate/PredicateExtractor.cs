using System.Collections.Generic;
using System.Linq;
using OpenConnector.CommandParser.Segment.DML.Column;
using OpenConnector.CommandParser.Segment.DML.Predicate;
using OpenConnector.CommandParser.Segment.Generic;
using OpenConnector.CommandParser.Segment.Generic.Table;
using OpenConnector.Exceptions;

namespace OpenConnector.CommandParser.Predicate
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 13:47:00
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class PredicateExtractor
    {
        private readonly ICollection<SimpleTableSegment> _tables;

        private readonly PredicateSegment _predicate;

        public PredicateExtractor(ICollection<SimpleTableSegment> tables, PredicateSegment predicate)
        {
            _tables = tables;
            _predicate = predicate;
        }

        /**
     * Extract tables.
     * 
     * @return table segments
     */
        public ICollection<SimpleTableSegment> ExtractTables()
        {
            ICollection<SimpleTableSegment> result = new LinkedList<SimpleTableSegment>();
            if (IsToGenerateTableTokenLeftValue())
            {
                if (_predicate.GetColumn().GetOwner() == null)
                    throw new ShardingException("predicate column owner can't found owner");
                var segment = _predicate.GetColumn().GetOwner();
                result.Add(new SimpleTableSegment(segment.GetStartIndex(), segment.GetStopIndex(), segment.GetIdentifier()));
            }
            if (IsToGenerateTableTokenForRightValue())
            {
                if (((ColumnSegment) _predicate.GetPredicateRightValue()).GetOwner() == null)
                    throw new ShardingException("predicate right value can't found owner");
                var segment = ((ColumnSegment)_predicate.GetPredicateRightValue()).GetOwner();
                result.Add(new SimpleTableSegment(segment.GetStartIndex(), segment.GetStopIndex(), segment.GetIdentifier()));
            }
            return result;
        }

        private bool IsToGenerateTableTokenLeftValue()
        {
            return _predicate.GetColumn().GetOwner()!=null && IsTable(_predicate.GetColumn().GetOwner());
        }

        private bool IsToGenerateTableTokenForRightValue()
        {
            return _predicate.GetPredicateRightValue() is ColumnSegment columnSegment
                && columnSegment.GetOwner()!=null && IsTable(columnSegment.GetOwner());
        }

        private bool IsTable(OwnerSegment owner)
        {
            var value = owner.GetIdentifier().GetValue();
            return !_tables.Any(table => value.Equals(table.GetAlias()));
        }
    }
}
