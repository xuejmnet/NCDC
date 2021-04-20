
namespace ShardingConnector.ParserBinder.Segment.Select.Projection.Impl
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
