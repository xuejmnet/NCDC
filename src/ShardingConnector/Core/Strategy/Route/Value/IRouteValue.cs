namespace ShardingConnector.Core.Strategy.Route.Value
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 00:00:00
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    /// 
    /// </summary>
    public interface IRouteValue
    {
        
        /**
     * Get column name.
     * 
     * @return column name
     */
        string GetColumnName();
    
        /**
     * Get table name.
     * 
     * @return table name
     */
        string GetTableName();
    }
}