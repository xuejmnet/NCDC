using NCDC.CommandParser.Common.Command.DML;
using NCDC.CommandParser.Common.Segment.DML.Predicate;
using NCDC.CommandParser.Common.Segment.Generic.Table;
using NCDC.Extensions;

namespace NCDC.CommandParser.Common.Util;

public static class WhereExtractUtil
{
    /**
     * Get join where segment from SelectCommand.
     *
     * @param selectCommand SelectCommand
     * @return join where segment collection
     */
    public static IEnumerable<WhereSegment> GetJoinWhereSegments( SelectCommand selectCommand) {

        if (selectCommand.From is not null)
        {
            return GetJoinWhereSegments(selectCommand.From);
        }

        return Array.Empty<WhereSegment>();

    }
    
    private static IEnumerable<WhereSegment> GetJoinWhereSegments(ITableSegment? tableSegment) {
        if (!(tableSegment is JoinTableSegment joinTableSegment) || null == joinTableSegment.Condition) {
            return Array.Empty<WhereSegment>();
        }
        ICollection<WhereSegment> result = new LinkedList<WhereSegment>();
        result.Add(GenerateWhereSegment(joinTableSegment));
        result.AddAll(GetJoinWhereSegments(joinTableSegment.Left));
        result.AddAll(GetJoinWhereSegments(joinTableSegment.Right));
        return result;
    }
    
    private static WhereSegment GenerateWhereSegment( JoinTableSegment joinTableSegment)
    {
        var expressionSegment = joinTableSegment.Condition!;
        return new WhereSegment(expressionSegment.StartIndex, expressionSegment.StopIndex, expressionSegment);
    }
    
    /**
     * Get subquery where segment from SelectCommand.
     *
     * @param selectCommand SelectCommand
     * @return subquery where segment collection
     */
    public static IEnumerable<WhereSegment> GetSubQueryWhereSegments(SelectCommand selectCommand) {
        var subQuerySegments = SubQueryExtractUtil.GetSubQuerySegments(selectCommand);
        foreach (var subQuerySegment in subQuerySegments)
        {
            if (subQuerySegment.Select.Where is  not null)
            {
                yield return subQuerySegment.Select.Where;
            }
        }
    }
}