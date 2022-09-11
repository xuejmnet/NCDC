namespace NCDC.CommandParserBinder.Segment.Select.Projection
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 8:47:44
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class DerivedColumn
    {

        private static readonly IDictionary<DerivedColumnEnum, DerivedColumn> _derivedColumns = new Dictionary<DerivedColumnEnum, DerivedColumn>();

        private static readonly ICollection<DerivedColumn> VALUES_WITHOUT_AGGREGATION_DISTINCT_DERIVED = GetValues();
        private readonly string _pattern;

        static DerivedColumn()
        {
            _derivedColumns.Add(DerivedColumnEnum.AVG_COUNT_ALIAS, new DerivedColumn("AVG_DERIVED_COUNT_"));
            _derivedColumns.Add(DerivedColumnEnum.AVG_SUM_ALIAS, new DerivedColumn("AVG_DERIVED_SUM_"));
            _derivedColumns.Add(DerivedColumnEnum.ORDER_BY_ALIAS, new DerivedColumn("ORDER_BY_DERIVED_"));
            _derivedColumns.Add(DerivedColumnEnum.GROUP_BY_ALIAS, new DerivedColumn("GROUP_BY_DERIVED_"));
            _derivedColumns.Add(DerivedColumnEnum.AGGREGATION_DISTINCT_DERIVED, new DerivedColumn("AGGREGATION_DISTINCT_DERIVED_"));
        }
        private DerivedColumn(string pattern)
        {
            _pattern = pattern;
        }

        public static DerivedColumn Get(DerivedColumnEnum derivedColumn)
        {
            return _derivedColumns[derivedColumn];
        }


        /// <summary>
        /// 获取派生行的别名<example>AVG_DERIVED_COUNT_2</example>
        /// </summary>
        /// <param name="derivedColumnCount"></param>
        /// <returns></returns>
        public string GetDerivedColumnAlias(int derivedColumnCount)
        {
            return $"{_pattern}{derivedColumnCount}";
        }

       /// <summary>
       /// 获取派生列名
       /// </summary>
       /// <param name="columnName"></param>
       /// <returns></returns>
        public static bool IsDerivedColumnName(string columnName)
        {
            return _derivedColumns.Any(col => columnName.StartsWith(col.Value._pattern));
        }

        /// <summary>
        /// 是否是派生行
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static bool IsDerivedColumn(string columnName)
        {
            return VALUES_WITHOUT_AGGREGATION_DISTINCT_DERIVED.Any(col => columnName.StartsWith(col._pattern));
        }
        /// <summary>
        /// 获取非聚合去重派生列
        /// </summary>
        /// <returns></returns>
        private static ICollection<DerivedColumn> GetValues()
        {
            ICollection<DerivedColumn> result = _derivedColumns
                .Where(o=>o.Key!= DerivedColumnEnum.AGGREGATION_DISTINCT_DERIVED)
                .Select(o=>o.Value).ToList();
            return result;
        }
    }

    public enum DerivedColumnEnum
    {
        AVG_COUNT_ALIAS=1,
        AVG_SUM_ALIAS=1<<1,
        ORDER_BY_ALIAS=1<<2,
        GROUP_BY_ALIAS=1<<3,
        AGGREGATION_DISTINCT_DERIVED=1<<4
    }
}
