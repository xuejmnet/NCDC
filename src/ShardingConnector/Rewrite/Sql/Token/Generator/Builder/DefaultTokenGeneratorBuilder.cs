using System;
using System.Collections.Generic;
using ShardingConnector.Rewrite.Sql.Token.Generator.Generic;

namespace ShardingConnector.Rewrite.Sql.Token.Generator.Builder
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 21:50:45
* @Email: 326308290@qq.com
*/
    public class DefaultTokenGeneratorBuilder:ISqlTokenGeneratorBuilder
    {
        public ICollection<ISqlTokenGenerator> GetSQLTokenGenerators()
        {
            ICollection<ISqlTokenGenerator> result = new LinkedList<ISqlTokenGenerator>();
            result.Add(new RemoveTokenGenerator());
            return result;
        }
    }
}