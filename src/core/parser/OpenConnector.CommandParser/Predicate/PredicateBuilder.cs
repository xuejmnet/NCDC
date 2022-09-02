using System.Collections.Generic;
using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParser.Constant;
using OpenConnector.CommandParser.Segment.Predicate;
using OpenConnector.Exceptions;
using OpenConnector.Extensions;


namespace OpenConnector.CommandParser.Predicate
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 13:53:57
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
   public sealed class PredicateBuilder
    {
        private readonly IASTNode _left;
    
    private readonly IASTNode _right;
    
    private readonly string _operator;

    public PredicateBuilder(IASTNode left, IASTNode right, string @operator)
    {
        _left = left;
        _right = right;
        _operator = @operator;
    }

    /**
         * Merge predicate.
         * 
         * @return Or predicate segment
         */
        public OrPredicateSegment MergePredicate()
        {
            var logicalOperator = LogicalOperator.ValueFrom(_operator);
            if (!logicalOperator.HasValue)
                throw new ShardingException($"operator:{_operator} cant found logical operator");
            return LogicalOperatorEnum.OR == logicalOperator.Value ? MergeOrPredicateSegment() : MergeAndPredicateSegment();
        }

        private OrPredicateSegment MergeOrPredicateSegment()
        {
            OrPredicateSegment result = new OrPredicateSegment();
            result.GetAndPredicates().AddAll(GetAndPredicates(_left));
            result.GetAndPredicates().AddAll(GetAndPredicates(_right));
            return result;
        }

        private OrPredicateSegment MergeAndPredicateSegment()
        {
            OrPredicateSegment result = new OrPredicateSegment();
            ICollection<AndPredicateSegment> leftPredicates = GetAndPredicates(_left);
            ICollection<AndPredicateSegment> rightPredicates = GetAndPredicates(_right);
            AddAndPredicates(result, leftPredicates, rightPredicates);
            return result;
        }

        private void AddAndPredicates(OrPredicateSegment orPredicateSegment,ICollection<AndPredicateSegment> leftPredicates,ICollection<AndPredicateSegment> rightPredicates)
        {
            if (0 == leftPredicates.Count && 0 == rightPredicates.Count)
            {
                return;
            }
            if (0 == leftPredicates.Count)
            {
                orPredicateSegment.GetAndPredicates().AddAll(rightPredicates);
            }
            if (0 == rightPredicates.Count)
            {
                orPredicateSegment.GetAndPredicates().AddAll(leftPredicates);
            }
            foreach (var leftPredicate in leftPredicates)
            {
                foreach (var rightPredicate in rightPredicates)
                {
                    orPredicateSegment.GetAndPredicates().Add(CreateAndPredicate(leftPredicate, rightPredicate));
                }
            }
        }

        private ICollection<AndPredicateSegment> GetAndPredicates(IASTNode astNode)
        {
            if (astNode is OrPredicateSegment orPredicateSegment) {
                return orPredicateSegment.GetAndPredicates();
            }
            if (astNode is AndPredicateSegment andPredicateSegment) {
                return new List<AndPredicateSegment>(){ andPredicateSegment };
            }
            if (astNode is PredicateSegment predicateSegment) {
                var andPredicate = new AndPredicateSegment();
                andPredicate.GetPredicates().Add(predicateSegment);
                return new List<AndPredicateSegment>() {andPredicate};
            }
            return new LinkedList<AndPredicateSegment>();
        }

        private AndPredicateSegment CreateAndPredicate(AndPredicateSegment left, AndPredicateSegment right)
        {
            AndPredicateSegment result = new AndPredicateSegment();
            result.GetPredicates().AddAll(left.GetPredicates());
            result.GetPredicates().AddAll(right.GetPredicates());
            return result;
        }
    }
}
