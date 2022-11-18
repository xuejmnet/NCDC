using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCDC.EntityFrameworkCore.Entities;

namespace NCDC.EntityFrameworkCore.Maps;

public class ActualDatabaseMap:BaseMap<ActualDatabaseEntity>
{
    protected override string TableName => "ActualDatabase";
    protected override void Configure0(EntityTypeBuilder<ActualDatabaseEntity> builder)
    {
        builder.Property(o => o.LogicDatabaseName).IsRequired().IsUnicode().HasMaxLength(255);
        builder.Property(o => o.DataSourceName).IsRequired().IsUnicode().HasMaxLength(255);
        builder.Property(o => o.ConnectionString).IsRequired().IsUnicode().HasMaxLength(255);
    }
}