using System;
using System.Text;
using ShardingConnector.Common.Rule;
using ShardingConnector.Extensions;
using ShardingConnector.RewriteEngine.Sql.Token.SimpleObject;
using ShardingConnector.RewriteEngine.Sql.Token.SimpleObject.Generic;
using ShardingConnector.Route.Context;

namespace ShardingConnector.ShardingRewrite.Token.SimpleObject
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 27 April 2021 20:27:23
* @Email: 326308290@qq.com
*/
    public sealed class ShardingInsertValuesToken : InsertValuesToken, IRouteUnitAware
    {
        public ShardingInsertValuesToken(int startIndex, int stopIndex) : base(startIndex, stopIndex)
        {
        }

        public string ToString(RouteUnit routeUnit)
        {
            StringBuilder result = new StringBuilder();
            AppendInsertValue(routeUnit, result);
            if (result.Length > 2)
                result.Remove(result.Length - 2, 2);
            return result.ToString();
        }

        private void AppendInsertValue(RouteUnit routeUnit, StringBuilder stringBuilder)
        {
            foreach (var insertValue in InsertValues)
            {
                if (IsAppend(routeUnit, (ShardingInsertValue) insertValue))
                {
                    stringBuilder.Append(insertValue).Append(", ");
                }
            }
        }

        private bool IsAppend(RouteUnit routeUnit, ShardingInsertValue insertValueToken)
        {
            if (insertValueToken.GetDataNodes().IsEmpty() || null == routeUnit)
            {
                return true;
            }

            foreach (var dataNode in insertValueToken.GetDataNodes())
            {
                if (routeUnit.FindTableMapper(dataNode.GetDataSourceName(), dataNode.GetTableName()) != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}