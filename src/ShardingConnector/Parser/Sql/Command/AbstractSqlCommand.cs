using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Kernels.Parse.SqlExpression;

namespace ShardingConnector.Parser.Sql.Command
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
        private int parameterCount;

        public int GetParameterCount()
        {
            return parameterCount;
        }
        public void SetParameterCount(int paramCount)
        {
            this.parameterCount = paramCount;
        }
    }
}
