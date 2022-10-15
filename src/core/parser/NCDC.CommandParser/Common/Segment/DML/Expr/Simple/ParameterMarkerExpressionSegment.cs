using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using NCDC.CommandParser.Common.Constant;
using NCDC.CommandParser.Common.Segment.DML.Item;
using NCDC.CommandParser.Common.Segment.Generic;

namespace NCDC.CommandParser.Common.Segment.DML.Expr.Simple
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 12:50:31
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class ParameterMarkerExpressionSegment:ISimpleExpressionSegment,IProjectionSegment,IAliasAvailable,IParameterMarkerSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        public int ParameterMarkerIndex { get; }
        public string ParamName { get; }
        public ParameterMarkerTypeEnum ParameterMarkerType { get; }
        private AliasSegment? _alias;


        public ParameterMarkerExpressionSegment(int startIndex, int stopIndex, int parameterMarkerIndex,ParameterMarkerTypeEnum parameterMarkerType,string paramName)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            ParameterMarkerIndex = parameterMarkerIndex;
            ParamName = NormalizeParameterName(paramName);
            ParameterMarkerType = parameterMarkerType;
        }

        public ParameterMarkerExpressionSegment(int startIndex, int stopIndex, int parameterMarkerIndex,string paramName)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            ParameterMarkerIndex = parameterMarkerIndex;
            ParamName = NormalizeParameterName(paramName);
            ParameterMarkerType = ParameterMarkerTypeEnum.AT;
        }

        internal static string NormalizeParameterName(string name)
        {
            return name.StartsWith("@", StringComparison.Ordinal) || name.StartsWith("?", StringComparison.Ordinal) ? name.Substring(1) : name;
        }

        public string? GetAlias()
        {
            return _alias?.IdentifierValue.Value;
        }

        public void SetAlias(AliasSegment? alias)
        {
            _alias = alias;
        }

        public override string ToString()
        {
            return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(ParameterMarkerIndex)}: {ParameterMarkerIndex}, {nameof(ParamName)}: {ParamName}, {nameof(ParameterMarkerType)}: {ParameterMarkerType}, Alias: {GetAlias()}";
        }
    }
}
