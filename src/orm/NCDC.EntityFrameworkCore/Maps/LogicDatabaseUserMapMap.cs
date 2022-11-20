using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCDC.EntityFrameworkCore.Entities;

namespace NCDC.EntityFrameworkCore.Maps;

public class LogicDatabaseUserMapMap:BaseMap<LogicDatabaseUserMapEntity>
{
    protected override string TableName => "logic_database_user_map";
    protected override void Configure0(EntityTypeBuilder<LogicDatabaseUserMapEntity> builder)
    {
        builder.Property(o => o.DatabaseId).IsRequired().HasMaxLength(50);
        builder.Property(o => o.AppAuthUserId).IsRequired().HasMaxLength(50);
    }
}