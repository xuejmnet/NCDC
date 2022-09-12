namespace NCDC.ShardingMerge.DataReaderMergers.DQL.GroupBy.Aggregation
{
/*
* @Author: xjm
* @Description:
* @Date: Friday, 07 May 2021 21:42:24
* @Email: 326308290@qq.com
*/
    public interface IAggregationUnit
    {
        
        /**
     * merge aggregation values.
     * 
     * @param values aggregation values
     */
        void Merge(List<IComparable> values);
    
        /**
     * Get aggregation result.
     * 
     * @return aggregation result
     */
        IComparable GetResult();
    }
}