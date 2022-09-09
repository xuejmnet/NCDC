using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParser.Constant;
using OpenConnector.CommandParser.Value.Identifier;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.MetaData;
using OpenConnector.Extensions;
using OpenConnector.Sharding.Rewrites.Sql.Token.SimpleObject;
using OpenConnector.Sharding.Routes;

namespace OpenConnector.Sharding.Rewrites.Token.SimpleObject
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/27 9:00:29
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class TableToken:SqlToken,ISubstitutable,IRouteUnitAware
    {
        private readonly int stopIndex;

        private readonly IdentifierValue identifier;
    
        private readonly ISqlCommandContext<ISqlCommand> sqlCommandContext;
        private readonly ITableMetadataManager _tableMetadataManager;


        public TableToken(int startIndex, int stopIndex, IdentifierValue identifier, ISqlCommandContext<ISqlCommand> sqlCommandContext, ITableMetadataManager tableMetadataManager) : base(startIndex)
        {
            this.stopIndex = stopIndex;
            this.identifier = identifier;
            this.sqlCommandContext = sqlCommandContext;
            _tableMetadataManager = tableMetadataManager;
        }

        public int GetStopIndex()
        {
            return stopIndex;
        }

        private IDictionary<string, string> GetLogicAndActualTables(RouteUnit routeUnit)
        {
            ICollection<string> tableNames = sqlCommandContext.GetTablesContext().GetTableNames();
            IDictionary<string, string> result = new Dictionary<string, string>(tableNames.Count);
            foreach (var tableMapper in routeUnit.TableMappers)
            {
                result.Add(tableMapper.LogicName.ToLower(), tableMapper.ActualName);
                result.AddAll(shardingRule.GetLogicAndActualTablesFromBindingTable(routeUnit.DataSourceMapper.LogicName, tableMapper.LogicName, tableMapper.ActualName, tableNames));
            }
            return result;
        }

        public string ToString(RouteUnit routeUnit)
        {
            String actualTableName = GetLogicAndActualTables(routeUnit)[identifier.GetValue().ToLower()];
            actualTableName = null == actualTableName ? identifier.GetValue().ToLower() : actualTableName;
            var quoteCharacterEnum = identifier.GetQuoteCharacter();
            return $"{QuoteCharacter.Get(quoteCharacterEnum).GetStartDelimiter()}{actualTableName}{QuoteCharacter.Get(quoteCharacterEnum).GetEndDelimiter()}";

        }
    }
}
