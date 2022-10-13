using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Common.Segment.DML.Expr.SubQuery;
using NCDC.CommandParser.Common.Segment.Generic;

namespace NCDC.CommandParser.Common.Segment.DML.Item
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/21 12:39:25
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SubQueryProjectionSegment:IProjectionSegment,IAliasAvailable
    {
        public SubQuerySegment SubQuery { get; }
        public string Text { get; }


        private AliasSegment? _alias;

        public SubQueryProjectionSegment(SubQuerySegment subQuery,string text)
        {
            SubQuery = subQuery;
            Text = text;
        }
        public string? GetAlias()
        {
            return _alias?.IdentifierValue.Value;
        }

        public void SetAlias(AliasSegment alias)
        {
            this._alias = alias;
        }

        public int StartIndex => SubQuery.StartIndex;
        public int StopIndex => SubQuery.StopIndex;

        public override string ToString()
        {
            return $"Alias: {GetAlias()}, {nameof(SubQuery)}: {SubQuery}, {nameof(Text)}: {Text}, {nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}";
        }
    }
}
