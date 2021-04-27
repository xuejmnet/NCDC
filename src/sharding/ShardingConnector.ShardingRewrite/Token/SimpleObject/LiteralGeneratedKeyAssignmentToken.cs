using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.ShardingRewrite.Token.SimpleObject
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/27 16:28:12
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class LiteralGeneratedKeyAssignmentToken:GeneratedKeyAssignmentToken
    {
        private readonly object value;
    
        public LiteralGeneratedKeyAssignmentToken(int startIndex, string columnName, object value) : base(startIndex, columnName)
        {
            this.value = value;
        }

        protected override string GetRightValue()
        {
            return value is string ? $"'{value}'" : value.ToString();
        }
    }
}
