
namespace NCDC.CommandParserBinder.Segment.Select.Projection.Impl
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 19:45:51
* @Email: 326308290@qq.com
*/
    public sealed class DerivedProjection:IProjection
    {
        private readonly string _expression;
    
        private readonly string _alias;

        public DerivedProjection(string expression, string @alias)
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