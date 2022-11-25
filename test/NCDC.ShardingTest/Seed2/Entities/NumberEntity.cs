namespace NCDC.ShardingTest.Seed2.Entities;

public class NumberEntity
{
    
    public string Id { get; set; }= null!;
    /// <summary>
    /// byte
    /// </summary>
    public byte Column1 { get; set; }
    public byte? Column2 { get; set; }
    
    /// <summary>
    /// varchar
    /// </summary>
    public sbyte Column3 { get; set; }
    public sbyte? Column4 { get; set; }
    
    
    /// <summary>
    /// tinytext
    /// </summary>
    public short Column5 { get; set; }
    public short? Column6 { get; set; }
    /// <summary>
    /// Text
    /// </summary>
    public ushort Column7 { get; set; }
    public ushort? Column8 { get; set; }
    /// <summary>
    /// MEDIUM TEXT
    /// </summary>
    public int Column9 { get; set; }
    public int? Column10 { get; set; }
    
    /// <summary>
    /// LONG TEXT
    /// </summary>
    public uint Column11 { get; set; }
    public uint? Column12 { get; set; }
    
    /// <summary>
    /// TINY BLOB
    /// </summary>
    public long Column13 { get; set; }
    public long? Column14 { get; set; }
    
    /// <summary>
    /// BLOB
    /// </summary>
    public ulong Column15 { get; set; }
    public ulong? Column16 { get; set; }
    
    
    /// <summary>
    /// MEDIUM BLOB
    /// </summary>
    public float Column17 { get; set; }
    public float? Column18 { get; set; }
    
    /// <summary>
    /// LONG BLOB
    /// </summary>
    public double Column19 { get; set; }
    public double? Column20 { get; set; }
    
    
    /// <summary>
    /// LONG BLOB
    /// </summary>
    public decimal Column21 { get; set; }
    public decimal? Column22 { get; set; }
}