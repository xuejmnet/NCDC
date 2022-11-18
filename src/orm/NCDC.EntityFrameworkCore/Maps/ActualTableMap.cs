using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCDC.EntityFrameworkCore.Entities;

namespace NCDC.EntityFrameworkCore.Maps;

public class ActualTableMap:BaseMap<ActualTableEntity>
{
    protected override string TableName => "ActualTable";
    protected override void Configure0(EntityTypeBuilder<ActualTableEntity> builder)
    {
        builder.Property(o => o.LogicDatabaseName).IsRequired().IsUnicode().HasMaxLength(255);
        builder.Property(o => o.LogicTableName).IsRequired().IsUnicode().HasMaxLength(255);
        builder.Property(o => o.DataSource).IsRequired().IsUnicode().HasMaxLength(255);
        builder.Property(o => o.TableName).IsRequired().IsUnicode().HasMaxLength(255);
    }
}