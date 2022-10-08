using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCDC.EntityFrameworkCore.Entities;

namespace NCDC.EntityFrameworkCore.Maps;

public class LogicTableMap:BaseMap<LogicTableEntity>
{
    public override string TableName => "LogicTable";
    protected override void Configure0(EntityTypeBuilder<LogicTableEntity> builder)
    {
        builder.Property(o => o.TableName).IsRequired().IsUnicode().HasMaxLength(255);
        builder.Property(o => o.Database).IsRequired().IsUnicode().HasMaxLength(255);
        builder.Property(o => o.ShardingDataSourceRule).IsUnicode().HasMaxLength(255);
        builder.Property(o => o.ShardingTableRule).IsUnicode().HasMaxLength(255);
    }
}