using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCDC.EntityFrameworkCore.Entities;

namespace NCDC.EntityFrameworkCore.Maps;

public class LogicDatabaseUserMap:BaseMap<LogicDatabaseUserEntity>
{
    public override string TableName => "LogicDatabaseUser";
    protected override void Configure0(EntityTypeBuilder<LogicDatabaseUserEntity> builder)
    {
        builder.Property(o => o.Database).IsRequired().IsUnicode().HasMaxLength(255);
        builder.Property(o => o.UserName).IsRequired().IsUnicode().HasMaxLength(255);
    }
}