using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCDC.EntityFrameworkCore.Entities;

namespace NCDC.EntityFrameworkCore.Maps;

public class DatabaseUserMap:BaseMap<DatabaseUserEntity>
{
    protected override string TableName => "database_user";
    protected override void Configure0(EntityTypeBuilder<DatabaseUserEntity> builder)
    {
        builder.Property(o => o.DatabaseId).IsRequired().HasMaxLength(50);
        builder.Property(o => o.AppAuthUserId).IsRequired().HasMaxLength(50);
    }
}