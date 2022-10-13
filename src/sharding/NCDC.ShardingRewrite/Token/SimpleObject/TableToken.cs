using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Constant;
using NCDC.CommandParser.Common.Value.Identifier;
using NCDC.ShardingParser.Command;
using NCDC.Basic.TableMetadataManagers;
using NCDC.CommandParser.Common.Command;
using NCDC.ShardingRewrite.Sql.Token.SimpleObject;
using NCDC.ShardingRoute;

namespace NCDC.ShardingRewrite.Token.SimpleObject
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
            int tableNameCount = sqlCommandContext.GetTablesContext().GetTableNameCount();
            IDictionary<string, string> result = new Dictionary<string, string>(tableNameCount);
            foreach (var tableMapper in routeUnit.TableMappers)
            {
                result.Add(tableMapper.LogicName.ToLower(), tableMapper.ActualName);
                // result.AddAll(shardingRule.GetLogicAndActualTablesFromBindingTable(routeUnit.DataSourceMapper.LogicName, tableMapper.LogicName, tableMapper.ActualName, tableNames));
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
