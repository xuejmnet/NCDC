using System;

namespace NCDC.CommandParser.Common.Segment.DML.Pagination.limit
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 19:54:32
* @Email: 326308290@qq.com
*/
    public sealed class ParameterMarkerLimitValueSegment:LimitValueSegment,IParameterMarkerPaginationValueSegment
    {
        public int ParameterIndex { get; }
        public  string ParameterName{ get; }

        public ParameterMarkerLimitValueSegment(int startIndex, int stopIndex,int parameterIndex,string paramName) : base(startIndex, stopIndex)
        {
            ParameterIndex = parameterIndex;
            ParameterName = NormalizeParameterName(paramName);
        }
        internal static string NormalizeParameterName(string name)
        {
            return name.StartsWith("@", StringComparison.Ordinal) || name.StartsWith("?", StringComparison.Ordinal) ? name.Substring(1) : name;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(ParameterIndex)}: {ParameterIndex}, {nameof(ParameterName)}: {ParameterName}";
        }
    }
}