using OpenConnector.Sharding.Rewrites.Sql.Token.SimpleObject;

namespace OpenConnector.Sharding.Rewrites.Token.SimpleObject
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/27 16:15:47
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class GeneratedKeyInsertColumnToken:SqlToken,IAttachable
    {
        private readonly string _column;
    
        public GeneratedKeyInsertColumnToken(int startIndex, string column):base(startIndex)
        {
            this._column = column;
        }


        public override string ToString()
        {
            return $", {_column}";
        }
    }
}
