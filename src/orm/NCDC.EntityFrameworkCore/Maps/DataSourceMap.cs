using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCDC.EntityFrameworkCore.Entities;

namespace NCDC.EntityFrameworkCore.Maps;

public class DataSourceMap:BaseMap<DataSourceEntity>
{
    public override string TableName => "DataSource";
    protected override void Configure0(EntityTypeBuilder<DataSourceEntity> builder)
    {
        builder.Property(o => o.Database).IsRequired().IsUnicode().HasMaxLength(255);
        builder.Property(o => o.Name).IsRequired().IsUnicode().HasMaxLength(255);
        builder.Property(o => o.ConnectionString).IsRequired().IsUnicode().HasMaxLength(255);
    }
}