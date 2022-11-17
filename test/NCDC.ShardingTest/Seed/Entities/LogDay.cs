namespace NCDC.ShardingTest.Seed.Entities;

public class LogDay
{
    public Guid Id { get; set; }
    public string LogLevel { get; set; }
    public string LogBody { get; set; }
    public DateTime LogTime { get; set; }
}