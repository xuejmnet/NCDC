

using OpenConnector.CommandParser.Abstractions;

namespace OpenConnector.CommandParser.Command
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
        private int _parameterCount;

        public virtual int GetParameterCount()
        {
            return _parameterCount;
        }
        public void SetParameterCount(int paramCount)
        {
            this._parameterCount = paramCount;
        }
    }
}
