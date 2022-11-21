using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCDC.EntityFrameworkCore.Entities;

namespace NCDC.EntityFrameworkCore.Maps;

public class ActualTableMap:BaseMap<ActualTableEntity>
{
    protected override string TableName => "actual_table";
    protected override void Configure0(EntityTypeBuilder<ActualTableEntity> builder)
    {
        builder.Property(o => o.LogicDatabaseId).IsRequired().HasMaxLength(50);
        builder.Property(o => o.LogicTableId).IsRequired().HasMaxLength(50);
        builder.Property(o => o.DataSourceId).IsRequired().HasMaxLength(50);
        builder.Property(o => o.TableName).IsRequired().HasMaxLength(255);
    }
}