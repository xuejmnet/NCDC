using NCDC.Basic.Parsers;
using NCDC.CommandParser.Abstractions;
using NCDC.ShardingParser.Command;
using NCDC.Basic.TableMetadataManagers;
using NCDC.ShardingRewrite.Sql.Token.Generator;
using NCDC.ShardingRewrite.Sql.Token.SimpleObject;
using NCDC.ShardingRewrite.Token.SimpleObject;

namespace NCDC.ShardingRewrite.Token.Generator.Impl
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/27 8:56:49
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class TableTokenGenerator:ICollectionSqlTokenGenerator<ISqlCommandContext<ISqlCommand>>
    {
        private readonly ITableMetadataManager _tableMetadataManager;

        public TableTokenGenerator(ITableMetadataManager tableMetadataManager)
        {
            _tableMetadataManager = tableMetadataManager;
        }
        public ICollection<SqlToken> GenerateSqlTokens(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            if (sqlCommandContext is ITableAvailable tableAvailable)
            {
                return GenerateSqlTokens(tableAvailable);
            }

            return new List<SqlToken>(0);
        }

        public bool IsGenerateSqlToken(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return true;
        }



        private ICollection<SqlToken> GenerateSqlTokens(ITableAvailable sqlStatementContext)
        {
            ICollection<SqlToken> result = new LinkedList<SqlToken>();
            foreach (var simpleTableSegment in sqlStatementContext.GetAllTables())
            {
                var tableMetadata = _tableMetadataManager.TryGet(simpleTableSegment.GetTableName().GetIdentifier().GetValue());
                if (tableMetadata!=null&&tableMetadata.IsMultiTableMapping)
                {
                    result.Add(new TableToken(simpleTableSegment.GetStartIndex(), simpleTableSegment.GetStopIndex(), simpleTableSegment.GetTableName().GetIdentifier(), (ISqlCommandContext<ISqlCommand>)sqlStatementContext, _tableMetadataManager));
                }
            }
            return result;
        }
    }
}
