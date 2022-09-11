using System.Text;
using NCDC.CommandParser.Segment.DML.Expr;
using NCDC.CommandParser.Segment.DML.Expr.Complex;
using NCDC.CommandParser.Segment.DML.Expr.Simple;

namespace NCDC.Sharding.Rewrites.Sql.Token.SimpleObject.Generic
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/24 10:48:23
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public  class InsertValue
    {
        private readonly List<IExpressionSegment> _values;

        public InsertValue(List<IExpressionSegment> values)
        {
            this._values = values;
        }
        public List<IExpressionSegment> GetValues()
        {
            return _values;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append("(");
            for (int i = 0; i < _values.Count; i++)
            {
                result.Append(GetValue(i)).Append(", ");
            }
            if (_values.Count > 0)
                result.Remove(result.Length - 2, 2);
            result.Append(")");
            return result.ToString();
        }


        private string GetValue(int index)
        {
            var expressionSegment = _values[index];
            if (expressionSegment is ParameterMarkerExpressionSegment)
            {
                return "?";
            }
            else if (expressionSegment is LiteralExpressionSegment)
            {
                Object literals = ((LiteralExpressionSegment)expressionSegment).GetLiterals();
                return literals is string ? $"'{((LiteralExpressionSegment)expressionSegment).GetLiterals()}'" : literals.ToString();
            }
            return ((IComplexExpressionSegment)expressionSegment).GetText();
        }
    }
}
