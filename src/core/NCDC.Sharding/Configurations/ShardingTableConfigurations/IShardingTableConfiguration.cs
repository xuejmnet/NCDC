namespace NCDC.Sharding.Configurations.ShardingTableConfigurations;

public interface IShardingTableConfiguration
{
    
     string LogicTable { get; }

     IDictionary<string/*data source name*/,string/*actual table name*/> ActualTables { get; }
}