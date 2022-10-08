using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCDC.EntityFrameworkCore.Entities;

namespace NCDC.EntityFrameworkCore.Maps;

public class AppAuthUserMap:BaseMap<AppAuthUserEntity>
{
    public override string TableName => "AppAuthUser";
    protected override void Configure0(EntityTypeBuilder<AppAuthUserEntity> builder)
    {
        builder.Property(o => o.UserName).IsRequired().IsUnicode().HasMaxLength(255);
        builder.Property(o => o.Password).IsRequired().IsUnicode().HasMaxLength(255);
        builder.Property(o => o.HostName).IsRequired().IsUnicode().HasMaxLength(255);
    }
}