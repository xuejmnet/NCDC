namespace NCDC.Basic.Metadatas;

public sealed class ColumnMetadata
{
    public ColumnMetadata(string name,int columnOrdinal, string dataTypeName, bool primaryKey, bool generated, bool caseSensitive)
    {
        
        Name = name;
        ColumnOrdinal = columnOrdinal;
        DataTypeName = dataTypeName;
        PrimaryKey = primaryKey;
        Generated = generated;
        CaseSensitive = caseSensitive;
    }
    protected bool Equals(ColumnMetadata other)
    {
        return Name == other.Name && ColumnOrdinal == other.ColumnOrdinal && DataTypeName == other.DataTypeName && PrimaryKey == other.PrimaryKey && Generated == other.Generated && CaseSensitive == other.CaseSensitive;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ColumnMetadata) obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (Name != null ? Name.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ ColumnOrdinal;
            hashCode = (hashCode * 397) ^ (DataTypeName != null ? DataTypeName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ PrimaryKey.GetHashCode();
            hashCode = (hashCode * 397) ^ Generated.GetHashCode();
            hashCode = (hashCode * 397) ^ CaseSensitive.GetHashCode();
            return hashCode;
        }
    }


    public  string Name { get; }
    public int ColumnOrdinal { get; }

    public string DataTypeName{ get; }
    
    public bool PrimaryKey{ get; }
    
    public bool Generated{ get; }
    
    public bool CaseSensitive{ get; }
}