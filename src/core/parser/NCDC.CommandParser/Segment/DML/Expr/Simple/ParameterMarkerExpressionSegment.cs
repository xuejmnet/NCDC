using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using OpenConnector.Base;

namespace NCDC.CommandParser.Segment.DML.Expr.Simple
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 12:50:31
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class ParameterMarkerExpressionSegment:ISimpleExpressionSegment
    {
        private readonly int _startIndex;

        private readonly int _stopIndex;

        private readonly int _parameterMarkerIndex;
        private readonly string _paramName;

        public ParameterMarkerExpressionSegment(int startIndex, int stopIndex, int parameterMarkerIndex,string paramName)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            _parameterMarkerIndex = parameterMarkerIndex;
            _paramName = NormalizeParameterName(paramName);
        }

        internal static string NormalizeParameterName(string name)
        {
            return name.StartsWith("@", StringComparison.Ordinal) || name.StartsWith("?", StringComparison.Ordinal) ? name.Substring(1) : name;
        }
        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public int GetParameterMarkerIndex()
        {
            return _parameterMarkerIndex;
        }

        public string GetParameterName()
        {
            return _paramName;
        }
    }
}
