using NCDC.CommandParser.Common.Segment.Generic;

namespace NCDC.CommandParser.Common.Segment.DML.Pagination
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 16:02:28
* @Email: 326308290@qq.com
*/
    public interface IParameterMarkerPaginationValueSegment:IPaginationValueSegment,IParameterMarkerSegment
    {
        int ParameterIndex { get; }
        string ParameterName{ get; }
    }
}