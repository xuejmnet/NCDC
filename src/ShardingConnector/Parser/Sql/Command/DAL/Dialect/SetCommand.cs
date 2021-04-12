using System;
using ShardingConnector.Parser.Sql.Segment.DAL;

namespace ShardingConnector.Parser.Sql.Command.DAL.Dialect
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 22:20:16
* @Email: 326308290@qq.com
*/
    public sealed class SetCommand:DALCommand
    {
        
        private VariableSegment variable;

        public VariableSegment GetVariable()
        {
            return variable;
        }

        public void SetVariable(VariableSegment variable)
        {
            this.variable = variable;
        }
    }
}