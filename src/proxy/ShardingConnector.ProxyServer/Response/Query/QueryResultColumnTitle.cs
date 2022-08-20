namespace ShardingConnector.ProxyServer.Response.Query;

public sealed class QueryResultColumnTitle
{
    public string Schema { get; }
    public string Table { get; }
    public string ColumnLabel { get; }
    public string ColumnName { get; }
    public int ColumnType { get; }
    public string ColumnTypeName { get; }
    public int ColumnLength { get; }
    public int Decimals { get; }
    public bool Signed { get; }
    public bool PrimaryKey { get; }
    public bool NotNull { get; }
    public bool AutoIncrement { get; }
    
    public QueryResultColumnTitle(string schema, string table, string columnLabel, string columnName, int columnType, string columnTypeName, int columnLength, int decimals, bool signed, bool primaryKey, bool notNull, bool autoIncrement)
    {
        Schema = schema;
        Table = table;
        ColumnLabel = columnLabel;
        ColumnName = columnName;
        ColumnType = columnType;
        ColumnTypeName = columnTypeName;
        ColumnLength = columnLength;
        Decimals = decimals;
        Signed = signed;
        PrimaryKey = primaryKey;
        NotNull = notNull;
        AutoIncrement = autoIncrement;
    }

}