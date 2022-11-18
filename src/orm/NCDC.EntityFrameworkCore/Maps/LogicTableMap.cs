using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCDC.EntityFrameworkCore.Entities;

namespace NCDC.EntityFrameworkCore.Maps;

public class LogicTableMap:BaseMap<LogicTableEntity>
{
    protected override string TableName => "LogicTable";
    protected override void Configure0(EntityTypeBuilder<LogicTableEntity> builder)
    {
        builder.Property(o => o.TableName).IsRequired().IsUnicode().HasMaxLength(255);
        builder.Property(o => o.LogicDatabaseName).IsRequired().IsUnicode().HasMaxLength(255);
        builder.Property(o => o.DataSourceRule).IsUnicode().HasMaxLength(255);
        builder.Property(o => o.TableRule).IsUnicode().HasMaxLength(255);
    }
}