﻿using NCDC.ShardingRewrite.Sql.Token.SimpleObject;

namespace NCDC.ShardingRewrite.Token.SimpleObject
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/27 16:13:44
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class DistinctProjectionPrefixToken:SqlToken,IAttachable
    {
        public DistinctProjectionPrefixToken(int startIndex) : base(startIndex)
        {
        }

        public override string ToString()
        {
            return "DISTINCT ";
        }
    }
}
