namespace NCDC.ShardingTest.Seed2.Entities;

public class DateTimeEntity
{
    
    
    public string Id { get; set; }= null!;
    /// <summary>
    /// byte
    /// </summary>
    public int Column1 { get; set; }
    public int? Column2 { get; set; }
    
    /// <summary>
    /// varchar
    /// </summary>
    public DateTime Column3 { get; set; }
    public DateTime? Column4 { get; set; }
    
    
    /// <summary>
    /// tinytext
    /// </summary>
    public DateTime Column5 { get; set; }
    public DateTime? Column6 { get; set; }
    /// <summary>
    /// Text
    /// </summary>
    public DateTime Column7 { get; set; }
    public DateTime? Column8 { get; set; }
    public TimeOnly Column9 { get; set; }
    public TimeOnly? Column10 { get; set; }
}