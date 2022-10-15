using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Common.Segment.DML.Expr.SubQuery;

namespace NCDC.CommandParser.Common.Segment.Generic.Table
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/21 13:04:48
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SubQueryTableSegment:ITableSegment
    {
        public SubQuerySegment SubQuery { get; }

        private AliasSegment? _alias;

        public SubQueryTableSegment(SubQuerySegment subQuery)
        {
            SubQuery = subQuery;
        }
        public string? GetAlias()
        {
            return _alias?.IdentifierValue.Value;
        }

        public void SetAlias(AliasSegment? alias)
        {
            this._alias = alias;
        }

        public int StartIndex => SubQuery.StartIndex;
        
        /// <summary>
        /// TODO _alias?.StopIndex??SubQuery.StopIndex;
        /// </summary>
        public int StopIndex => SubQuery.StopIndex;
    }
}
