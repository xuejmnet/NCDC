using System;

namespace NCDC.CommandParser.Segment.DML.Pagination.limit
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 19:54:32
* @Email: 326308290@qq.com
*/
    public sealed class ParameterMarkerLimitValueSegment:LimitValueSegment,IParameterMarkerPaginationValueSegment
    {
        private readonly int _parameterIndex;
        private readonly string _paramName;

        public ParameterMarkerLimitValueSegment(int startIndex, int stopIndex,int parameterIndex,string paramName) : base(startIndex, stopIndex)
        {
            _parameterIndex = parameterIndex;
            _paramName = NormalizeParameterName(paramName);
            
        }
        internal static string NormalizeParameterName(string name)
        {
            return name.StartsWith("@", StringComparison.Ordinal) || name.StartsWith("?", StringComparison.Ordinal) ? name.Substring(1) : name;
        }

        public int GetParameterIndex()
        {
            return _parameterIndex;
        }
        public string GetParameterName()
        {
            return _paramName;
        }
    }
}