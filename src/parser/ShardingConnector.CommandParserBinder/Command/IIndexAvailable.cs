using ShardingConnector.CommandParser.Segment.DDL.Index;

namespace ShardingConnector.CommandParserBinder.Command
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 27 April 2021 21:50:06
* @Email: 326308290@qq.com
*/
    public interface IIndexAvailable
    {
        ICollection<IndexSegment> GetIndexes();
    }
}