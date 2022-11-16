namespace NCDC.ShardingTest.Seed.Entities;

public class TestNumber
{
    public int Id { get; set; }
    public int Seq { get; set; }
    public decimal Money { get; set; }
    public string Remark { get; set; } = null!;
}