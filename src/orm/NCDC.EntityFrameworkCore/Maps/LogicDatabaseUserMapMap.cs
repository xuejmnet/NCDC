using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCDC.EntityFrameworkCore.Entities;

namespace NCDC.EntityFrameworkCore.Maps;

public class LogicDatabaseUserMapMap:BaseMap<LogicDatabaseUserMapEntity>
{
    protected override string TableName => "LogicDatabaseUserMap";
    protected override void Configure0(EntityTypeBuilder<LogicDatabaseUserMapEntity> builder)
    {
        builder.Property(o => o.DatabaseName).IsRequired().IsUnicode().HasMaxLength(255);
        builder.Property(o => o.UserName).IsRequired().IsUnicode().HasMaxLength(255);
    }
}