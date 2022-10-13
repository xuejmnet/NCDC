using NCDC.CommandParser.Common.Segment.DML.Predicate;

namespace NCDC.ShardingParser.Command
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 14:53:12
* @Email: 326308290@qq.com
*/
    public interface IWhereAvailable
    {
        
        WhereSegment? GetWhere();
    }
}