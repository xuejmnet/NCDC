using System;
using System.Collections.Generic;
using System.Text;
using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParser.Command;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.MetaData.Schema;

namespace OpenConnector.RewriteEngine.Parameter.Rewrite
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
