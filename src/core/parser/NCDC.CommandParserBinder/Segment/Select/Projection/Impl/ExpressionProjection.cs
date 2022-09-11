
namespace NCDC.CommandParserBinder.Segment.Select.Projection.Impl
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 8:38:54
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ExpressionProjection:IProjection
    {
        private readonly string _expression;
    
        private readonly string _alias;

        public ExpressionProjection(string expression, string @alias)
        {
            _expression = expression;
            _alias = alias;
        }

        public string GetExpression()
        {
            return _expression;
        }

        private bool Equals(ExpressionProjection other)
        {
            return _expression == other._expression && _alias == other._alias;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is ExpressionProjection other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_expression != null ? _expression.GetHashCode() : 0) * 397) ^ (_alias != null ? _alias.GetHashCode() : 0);
            }
        }

        public string GetAlias()
        {
            return _alias;
        }

        public string GetColumnLabel()
        {
            return _alias ?? GetExpression();
        }
    }
}
