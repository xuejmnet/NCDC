namespace NCDC.ShardingRewrite.Sql.Token.SimpleObject
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 13 April 2021 21:00:11
* @Email: 326308290@qq.com
*/
    public interface ISubstitutable
    {
        
        int GetStartIndex();
    
        /**
     * Get stop index.
     * 
     * @return stop index
     */
        int GetStopIndex();
    }
}