namespace NCDC.Sharding.Rewrites.Token.SimpleObject
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/27 16:41:46
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ParameterMarkerGeneratedKeyAssignmentToken:GeneratedKeyAssignmentToken
    {
        public ParameterMarkerGeneratedKeyAssignmentToken(int startIndex, string columnName) : base(startIndex, columnName)
        {
        }

        protected override string GetRightValue()
        {
            return "?";
        }
    }
}
