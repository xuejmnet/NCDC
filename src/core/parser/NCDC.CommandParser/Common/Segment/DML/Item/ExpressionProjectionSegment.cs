using NCDC.CommandParser.Common.Segment.DML.Expr;
using NCDC.CommandParser.Common.Segment.DML.Expr.Complex;
using NCDC.CommandParser.Common.Segment.Generic;
using NCDC.CommandParser.Common.Util;

namespace NCDC.CommandParser.Common.Segment.DML.Item
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 8:10:35
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ExpressionProjectionSegment:IProjectionSegment,IComplexExpressionSegment,IAliasAvailable
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        public IExpressionSegment? Expr { get; }
        public string Text { get; }
    
        private AliasSegment? _alias;

        public ExpressionProjectionSegment(int startIndex, int stopIndex, string text,IExpressionSegment? expr)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            Text = SqlUtil.GetExpressionWithoutOutsideParentheses(text);
            Expr = expr;
        }

        public string? GetAlias()
        {
            return _alias?.IdentifierValue.Value;
        }


        public void SetAlias(AliasSegment? alias)
        {
            this._alias = alias;
        }

        public override string ToString()
        {
            return $"Alias: {GetAlias()}, {nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(Expr)}: {Expr}, {nameof(Text)}: {Text}";
        }
    }
}
