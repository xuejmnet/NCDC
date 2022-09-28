using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCDC.EntityFrameworkCore.Entities;

namespace NCDC.EntityFrameworkCore.Maps;

public class LogicDatabaseMap:BaseMap<LogicDatabaseEntity>
{
    public override string TableName => "LogicDatabase";
    protected override void Configure0(EntityTypeBuilder<LogicDatabaseEntity> builder)
    {
        builder.Property(o => o.Name).IsRequired().IsUnicode(true).HasMaxLength(50);
        builder.Property(o => o.ConnectionMode).HasConversion<int>();
        builder.HasIndex(o => o.Name);
    }
}