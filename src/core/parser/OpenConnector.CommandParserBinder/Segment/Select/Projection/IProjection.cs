namespace OpenConnector.CommandParserBinder.Segment.Select.Projection
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 14:55:59
* @Email: 326308290@qq.com
*/
    public interface IProjection
    {
        
        string GetExpression();
    
        string GetAlias();

        string GetColumnLabel();
    }
}