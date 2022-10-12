using NCDC.CommandParser.Abstractions;


namespace NCDC.CommandParser.Segment
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/10 9:49:09
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface ISqlSegment:IASTNode
    {
        /// <summary>
        /// 起始索引
        /// </summary>
        /// <returns></returns>
        int StartIndex { get; }

        /// <summary>
        /// 结束索引
        /// </summary>
        /// <returns></returns>
        int StopIndex { get; }
    }
}
