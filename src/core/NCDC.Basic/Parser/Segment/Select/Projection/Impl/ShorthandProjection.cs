namespace NCDC.Basic.Parser.Segment.Select.Projection.Impl
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 15:09:50
* @Email: 326308290@qq.com
*/
    public sealed class ShorthandProjection:IProjection
    {
        private readonly string _owner;
    
        private readonly ICollection<ColumnProjection> _actualColumns;

        public ShorthandProjection(string owner, ICollection<ColumnProjection> actualColumns)
        {
            _owner = owner;
            _actualColumns = actualColumns;
        }

        public string GetExpression()
        {
            return string.IsNullOrEmpty(_owner) ? "*" : _owner + ".*";
        }

        public string GetAlias()
        {
            return null;
        }

        public string GetColumnLabel()
        {
            return "*";
        }

        public string GetOwner()
        {
            return _owner;
        }

        public ICollection<ColumnProjection> GetActualColumns()
        {
            return _actualColumns;
        }
    }
}