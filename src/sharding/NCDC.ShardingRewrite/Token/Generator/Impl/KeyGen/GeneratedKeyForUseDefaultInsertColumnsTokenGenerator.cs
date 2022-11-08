using NCDC.Base;
using NCDC.CommandParser.Common.Command.DML;
using NCDC.ShardingParser.Command.DML;
using NCDC.ShardingParser.Extensions;
using NCDC.ShardingParser.MetaData;
using NCDC.ShardingRewrite.Sql.Token.SimpleObject;
using NCDC.ShardingRewrite.Sql.Token.SimpleObject.Generic;

namespace NCDC.ShardingRewrite.Token.Generator.Impl.KeyGen
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 27 April 2021 20:59:20
* @Email: 326308290@qq.com
*/
    public sealed class GeneratedKeyForUseDefaultInsertColumnsTokenGenerator:BaseGeneratedKeyTokenGenerator
    {
        private readonly ITableMetadataManager _tableMetadataManager;

        public GeneratedKeyForUseDefaultInsertColumnsTokenGenerator(ITableMetadataManager tableMetadataManager)
        {
            _tableMetadataManager = tableMetadataManager;
        }
        public override SqlToken GenerateSqlToken(InsertCommandContext sqlCommandContext)
        {
            var insertColumnsSegment = sqlCommandContext.GetSqlCommand().InsertColumns;
            ShardingAssert.ShouldBeNotNull(insertColumnsSegment,"insertColumnsSegment is required");
            return new UseDefaultInsertColumnsToken(insertColumnsSegment.StopIndex, GetColumnNames(sqlCommandContext));
        }

        public override bool IsGenerateSqlToken(InsertCommandContext insertCommandContext)
        {
            return !insertCommandContext.ContainsInsertColumns();
        }

        private List<String> GetColumnNames(InsertCommandContext insertCommandContext) {
            var generatedKey = insertCommandContext.GetGeneratedKeyContext();
            ShardingAssert.ShouldBeNotNull(generatedKey,"generatedKey is required");
            List<String> result = new List<string>(insertCommandContext.GetColumnNames(_tableMetadataManager));
            result.Remove(generatedKey.GetColumnName());
            result.Add(generatedKey.GetColumnName());
            return result;
        }
    }
}