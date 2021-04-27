using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Constant;
using ShardingConnector.CommandParser.Value.Identifier;
using ShardingConnector.Extensions;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.RewriteEngine.Sql.Token.SimpleObject;
using ShardingConnector.Route.Context;
using ShardingConnector.ShardingCommon.Core.Rule;

namespace ShardingConnector.ShardingRewrite.Token.SimpleObject
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/27 16:17:26
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class IndexToken:SqlToken,ISubstitutable,IRouteUnitAware
    {
        private readonly int _stopIndex;

        private readonly IdentifierValue _identifier;
    
        private readonly ISqlCommandContext<ISqlCommand> _sqlCommandContext;
    
        private readonly ShardingRule _shardingRule;
        public IndexToken(int startIndex, int stopIndex, IdentifierValue identifier, ISqlCommandContext<ISqlCommand> sqlCommandContext, ShardingRule shardingRule) : base(startIndex)
        {
            this._stopIndex = stopIndex;
            this._identifier = identifier;
            this._sqlCommandContext = sqlCommandContext;
            this._shardingRule = shardingRule;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public string ToString(RouteUnit routeUnit)
        {
            StringBuilder result = new StringBuilder();
            var quoteCharacterEnum = _identifier.GetQuoteCharacter();
            var quoteCharacter = QuoteCharacter.Get(quoteCharacterEnum);
            result.Append(quoteCharacter.GetStartDelimiter()).Append(_identifier.GetValue());
            IDictionary<string, string> logicAndActualTables = GetLogicAndActualTables(routeUnit);
            if (logicAndActualTables.Any())
            {
                result.Append("_").Append(logicAndActualTables.Values.FirstOrDefault());
            }
            result.Append(quoteCharacter.GetEndDelimiter());
            return result.ToString();
        }

        private IDictionary<string, string> GetLogicAndActualTables(RouteUnit routeUnit)
        {
            ICollection<string> tableNames = _sqlCommandContext.GetTablesContext().GetTableNames();
            IDictionary<string, string> result = new Dictionary<string, string>(tableNames.Count);

            foreach (var tableMapper in routeUnit.TableMappers)
            {
                result.Add(tableMapper.LogicName.ToLower(),tableMapper.ActualName);
                result.AddAll(_shardingRule.GetLogicAndActualTablesFromBindingTable(routeUnit.DataSourceMapper.LogicName,tableMapper.LogicName,tableMapper.ActualName,tableNames));
            }
            return result;
        }
    }
}
