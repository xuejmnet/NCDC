

using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Segment.Generic;

namespace NCDC.CommandParser.Common.Command
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/10 9:31:13
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public abstract class AbstractSqlCommand : ISqlCommand
    {
        public  int ParameterCount { get; set; }
        public ICollection<IParameterMarkerSegment> ParameterMarkerSegments = new LinkedList<IParameterMarkerSegment>();
        public ICollection<CommentSegment>  CommentSegments= new LinkedList<CommentSegment>();
    }
}
