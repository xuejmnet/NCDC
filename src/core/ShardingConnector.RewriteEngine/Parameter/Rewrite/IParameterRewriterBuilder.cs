using System;
using System.Collections.Generic;
using System.Text;

using ShardingConnector.CommandParser.Command;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.MetaData.Schema;

namespace ShardingConnector.RewriteEngine.Parameter.Rewrite
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/26 15:06:48
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IParameterRewriterBuilder
    {
        ICollection<IParameterRewriter<ISqlCommandContext<ISqlCommand>>> GetParameterRewriters(SchemaMetaData schemaMetaData);
    }
}
