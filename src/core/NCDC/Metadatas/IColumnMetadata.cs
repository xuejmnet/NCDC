namespace OpenConnector.Metadatas;

public interface IColumnMetadata
{
    string Name { get; }
    int ColumnIndex { get; }
    string DataTypeName { get; }
    bool PrimaryKey { get; }
    bool Generated { get; }
    bool CaseSensitive { get; }
}