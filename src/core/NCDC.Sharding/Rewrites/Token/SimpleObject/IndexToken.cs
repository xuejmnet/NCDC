using System.Text;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Constant;
using NCDC.CommandParser.Value.Identifier;
using NCDC.Basic.Parser.Command;
using NCDC.Basic.Parser.MetaData;
using NCDC.Basic.TableMetadataManagers;
using OpenConnector.Extensions;
using NCDC.Sharding.Rewrites.Sql.Token.SimpleObject;
using NCDC.Sharding.Routes;

namespace NCDC.Sharding.Rewrites.Token.SimpleObject
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
        private readonly ITableMetadataManager _tableMetadataManager;

        public IndexToken(int startIndex, int stopIndex, IdentifierValue identifier, ISqlCommandContext<ISqlCommand> sqlCommandContext, ITableMetadataManager tableMetadataManager) : base(startIndex)
        {
            this._stopIndex = stopIndex;
            this._identifier = identifier;
            this._sqlCommandContext = sqlCommandContext;
            _tableMetadataManager = tableMetadataManager;
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
            int tableNameCount = _sqlCommandContext.GetTablesContext().GetTableNameCount();
            IDictionary<string, string> result = new Dictionary<string, string>(tableNameCount);

            foreach (var tableMapper in routeUnit.TableMappers)
            {
                result.Add(tableMapper.LogicName.ToLower(),tableMapper.ActualName);
                // result.AddAll(_shardingRule.GetLogicAndActualTablesFromBindingTable(routeUnit.DataSourceMapper.LogicName,tableMapper.LogicName,tableMapper.ActualName,tableNames));
            }
            return result;
        }
    }
}
