using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Common.Segment.Generic;
using NCDC.CommandParser.Common.Value.Identifier;

namespace NCDC.CommandParser.Common.Segment.DDL.Index
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 17:11:03
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class IndexSegment:ISqlSegment,IOwnerAvailable
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        public IndexNameSegment IndexName { get; }
        public OwnerSegment? Owner { get; set; }

        public IndexSegment(int startIndex, int stopIndex, IndexNameSegment indexName)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            IndexName = indexName;
        }
    }
}
