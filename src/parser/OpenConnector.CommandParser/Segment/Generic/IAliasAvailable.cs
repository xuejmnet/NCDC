namespace OpenConnector.CommandParser.Segment.Generic
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/10 9:52:19
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IAliasAvailable: ISqlSegment
    {
        string GetAlias();
        void SetAlias(AliasSegment alias);
    }
}
