
namespace NCDC.ShardingParser.Segment.Select.Projection.Impl
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 15:11:27
* @Email: 326308290@qq.com
*/
    public sealed class ColumnProjection:IProjection
    {
        private readonly string? _owner;
        private readonly string _name;
        private readonly string? _alias;


        public ColumnProjection(string? owner, string name, string? @alias)
        {
            _owner = owner;
            _name = name;
            _alias = alias;
        }

        public string GetExpression()
        {
            return null == _owner ? _name : _owner + "." + _name;
        }

        public string? GetAlias()
        {
            return _alias;
        }

        public string GetColumnLabel()
        {
            return _alias ?? _name;
        }

        public string GetName()
        {
            return _name;
        }
        /// <summary>
        /// table.column as alias
        /// </summary>
        /// <returns></returns>
        public string GetExpressionWithAlias() {
            return GetExpression() + (null == _alias ? "" : " AS " + _alias);
        }

    }
}