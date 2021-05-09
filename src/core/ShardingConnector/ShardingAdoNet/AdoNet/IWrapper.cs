namespace ShardingConnector.ShardingAdoNet.AdoNet
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/14 00:00:00
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    /// 
    /// </summary>
    public interface IWrapper
    {
        T Unwrap<T>();
    }
}