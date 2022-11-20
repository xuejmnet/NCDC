using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCDC.EntityFrameworkCore.Entities;

namespace NCDC.EntityFrameworkCore.Maps;

public class LogicDatabaseMap:BaseMap<LogicDatabaseEntity>
{
    protected override string TableName => "logic_database";
    protected override void Configure0(EntityTypeBuilder<LogicDatabaseEntity> builder)
    {
        builder.Property(o => o.DatabaseName).IsRequired().HasMaxLength(355);
        builder.Property(o => o.ConnectionMode).HasConversion<int>();
    }
}