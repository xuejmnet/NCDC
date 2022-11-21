using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCDC.EntityFrameworkCore.Entities;

namespace NCDC.EntityFrameworkCore.Maps;

public class AppAuthUserMap:BaseMap<AppAuthUserEntity>
{
    protected override string TableName => "app_auth_user";
    protected override void Configure0(EntityTypeBuilder<AppAuthUserEntity> builder)
    {
        builder.Property(o => o.UserName).IsRequired().HasMaxLength(255);
        builder.Property(o => o.Password).IsRequired().HasMaxLength(255);
        builder.Property(o => o.HostName).IsRequired().HasMaxLength(255);
    }
}