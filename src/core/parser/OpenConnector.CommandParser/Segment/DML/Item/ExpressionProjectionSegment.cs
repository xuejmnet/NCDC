using OpenConnector.CommandParser.Segment.DML.Expr.Complex;
using OpenConnector.CommandParser.Segment.Generic;
using OpenConnector.CommandParser.Util;

namespace OpenConnector.CommandParser.Segment.DML.Item
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
        private readonly int _startIndex;

        private readonly int _stopIndex;

        private readonly string _text;
    
        private AliasSegment alias;

        public ExpressionProjectionSegment(int startIndex, int stopIndex, string text)
        {
            _startIndex = startIndex;
            _stopIndex = stopIndex;
            _text = SqlUtil.GetExpressionWithoutOutsideParentheses(text);
        }

        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public string GetText()
        {
            return _text;
        }

        public string GetAlias()
        {
            return alias?.GetIdentifier().GetValue();
        }


        public void SetAlias(AliasSegment alias)
        {
            this.alias = alias;
        }
    }
}
