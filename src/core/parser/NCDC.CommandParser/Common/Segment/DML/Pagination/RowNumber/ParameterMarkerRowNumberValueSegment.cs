using System;

namespace NCDC.CommandParser.Common.Segment.DML.Pagination.RowNumber
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 12:25:22
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ParameterMarkerRowNumberValueSegment:RowNumberValueSegment,IParameterMarkerPaginationValueSegment
    {

        public int ParameterIndex { get; }
        public string ParameterName { get; }
        public ParameterMarkerRowNumberValueSegment(int startIndex, int stopIndex,int parameterIndex, string parameterName, bool boundOpened) : base(startIndex, stopIndex, boundOpened)
        {
            ParameterIndex = parameterIndex;
            ParameterName = NormalizeParameterName(parameterName);
        }
        internal static string NormalizeParameterName(string name)
        {
            return name.StartsWith("@", StringComparison.Ordinal) || name.StartsWith("?", StringComparison.Ordinal) ? name.Substring(1) : name;
        }

    }
}
