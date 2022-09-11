namespace OpenConnector.Metadatas;

public interface IDatabaseMetadata
{
    List<string> GetTableNames();
    ITableMetadata? GetTable(string tableName);
    bool Add(string tableName, ITableMetadata tableMetaData);
    bool Remove(string tableName);
    bool ContainsTable(string tableName);
    bool ContainsColumn(string tableName, string columnName);
    List<string> GetAllColumnNames(string tableName);
}