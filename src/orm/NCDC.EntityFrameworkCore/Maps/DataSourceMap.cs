using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCDC.EntityFrameworkCore.Entities;

namespace NCDC.EntityFrameworkCore.Maps;

public class DataSourceMap:BaseMap<DataSourceEntity>
{
    public override string TableName => "DataSource";
    protected override void Configure0(EntityTypeBuilder<DataSourceEntity> builder)
    {
        builder.Property(o => o.Database).IsRequired().IsUnicode(false).HasMaxLength(100);
        builder.Property(o => o.Name).IsRequired().IsUnicode(true).HasMaxLength(50);
        builder.Property(o => o.ConnectionString).IsRequired().IsUnicode(false).HasMaxLength(100);
        builder.HasIndex(o => o.Database);
    }
}