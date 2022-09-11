namespace NCDC.CommandParserBinder.Segment.Insert.Keygen
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 9:01:58
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class GeneratedKeyContext
    {
        private readonly string _columnName;
    
        private readonly bool _generated;
    
        private readonly ICollection<IComparable> _generatedValues = new LinkedList<IComparable>();

        public GeneratedKeyContext(string columnName, bool generated)
        {
            _columnName = columnName;
            _generated = generated;
        }

        public string GetColumnName()
        {
            return _columnName;
        }

        public bool IsGenerated()
        {
            return _generated;
        }

        public ICollection<IComparable> GetGeneratedValues()
        {
            return _generatedValues;
        }
    }
}
