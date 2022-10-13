using NCDC.CommandParser.Common.Segment.DML.Column;
using NCDC.CommandParser.Common.Segment.DML.Expr.SubQuery;
using NCDC.CommandParser.Common.Segment.DML.Order;

namespace NCDC.CommandParser.Common.Segment.Generic;

public sealed class ModelSegment:ISqlSegment
{
    public ModelSegment(int startIndex, int stopIndex)
    {
        StartIndex = startIndex;
        StopIndex = stopIndex;
    }

    public int StartIndex { get; }
    public int StopIndex { get; }
    
    public  ICollection<SubQuerySegment> ReferenceModelSelects = new LinkedList<SubQuerySegment>();
    
    public  ICollection<OrderBySegment> OrderBySegments = new LinkedList<OrderBySegment>();
    
    public  ICollection<ColumnSegment> CellAssignmentColumns = new LinkedList<ColumnSegment>();
    
    public  ICollection<SubQuerySegment> CellAssignmentSelects = new LinkedList<SubQuerySegment>();
}