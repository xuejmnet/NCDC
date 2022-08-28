using ShardingConnector.CommandParser.Segment.Predicate;

namespace ShardingConnector.CommandParserBinder.Command
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 14:53:12
* @Email: 326308290@qq.com
*/
    public interface IWhereAvailable
    {
        
        WhereSegment GetWhere();
    }
}