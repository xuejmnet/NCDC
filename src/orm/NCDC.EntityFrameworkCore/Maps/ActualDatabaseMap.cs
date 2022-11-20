using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCDC.EntityFrameworkCore.Entities;

namespace NCDC.EntityFrameworkCore.Maps;

public class ActualDatabaseMap:BaseMap<ActualDatabaseEntity>
{
    protected override string TableName => "actual_database";
    protected override void Configure0(EntityTypeBuilder<ActualDatabaseEntity> builder)
    {
        builder.Property(o => o.LogicDatabaseId).IsRequired().HasMaxLength(50);
        builder.Property(o => o.DataSourceName).IsRequired().HasMaxLength(255);
        builder.Property(o => o.ConnectionString).IsRequired().HasMaxLength(255);
    }
}