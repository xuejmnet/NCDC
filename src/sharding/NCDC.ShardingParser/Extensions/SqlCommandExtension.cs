using NCDC.ShardingParser.Command.DML;
using NCDC.ShardingParser.MetaData;

namespace NCDC.ShardingParser.Extensions;

public static class SqlCommandExtension
{
    public static IReadOnlyList<string> GetColumnNames(this InsertCommandContext insertCommandContext,ITableMetadataManager tableMetadataManager)
    {
        var containsInsertColumns = insertCommandContext.ContainsInsertColumns();
        if (containsInsertColumns)
        {
            return insertCommandContext.InsertColumnNames;
            // var tableName = GetSqlCommand()?.Table?.TableName.IdentifierValue.Value;
            // _columnNames = ContainsInsertColumns() ? InsertColumnNames : new List<string>(0);//(tableName is not null? tableMetadataManager.GetAllColumnNames(tableName):new List<string>());

        }
        else
        {
            var tableName = insertCommandContext.GetSqlCommand()?.Table?.TableName.IdentifierValue.Value;
            if (tableName is not null)
            {
                return tableMetadataManager.GetAllColumnNames(tableName);
            }
        }

        return new List<string>(0);
    }
}