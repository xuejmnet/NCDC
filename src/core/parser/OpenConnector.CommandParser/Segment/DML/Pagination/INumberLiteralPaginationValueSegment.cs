namespace OpenConnector.CommandParser.Segment.DML.Pagination
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 11 April 2021 16:04:49
* @Email: 326308290@qq.com
*/
    public interface INumberLiteralPaginationValueSegment:IPaginationValueSegment
    {
        long GetValue();
    }
}