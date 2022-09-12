using NCDC.CommandParser.Abstractions;

namespace NCDC.CommandParser.SqlParseEngines.Hook
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 10:03:17
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IParsingHook
    {
        void Start(String sql);


        void FinishSuccess(ISqlCommand sqlCommand);


        void FinishFailure(Exception exception);
    }
}
