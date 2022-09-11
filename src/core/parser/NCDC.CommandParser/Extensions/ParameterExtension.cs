using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using OpenConnector.Base;
using NCDC.CommandParser.Segment.DML.Expr.Simple;

namespace NCDC.CommandParser.Extensions
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/7/26 11:24:30
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public static  class ParameterExtension
    {

        public static object GetParameterValue(this List<object> parameters, ParameterMarkerExpressionSegment parameterMarkerExpressionSegment)
        {

            var parameter = parameters.Select(o => (DbParameter)o).FirstOrDefault(o => o.ParameterName.Equals(parameterMarkerExpressionSegment.GetParameterName()));
            ShardingAssert.ShouldBeNotNull(parameter, $"Sharding value cant find parameter name :[{parameterMarkerExpressionSegment.GetParameterName()}].");
            return parameter.Value;
        }
    }
}
