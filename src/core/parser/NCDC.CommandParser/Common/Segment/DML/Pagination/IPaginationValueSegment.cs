namespace NCDC.CommandParser.Common.Segment.DML.Pagination
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 14:34:41
* @Email: 326308290@qq.com
*/
    public interface IPaginationValueSegment : ISqlSegment
    {
        /**
     * Is bound opened.
     * 
     * @return bound opened
     */
        bool IsBoundOpened();
    }
}