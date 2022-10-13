namespace NCDC.CommandParser.Common.Segment.Generic
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/10 9:52:37
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IOwnerAvailable:ISqlSegment
    {
        OwnerSegment? Owner { get; set; }
    }
}
