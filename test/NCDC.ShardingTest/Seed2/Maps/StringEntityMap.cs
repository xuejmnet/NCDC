using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCDC.ShardingTest.Seed2.Entities;

namespace NCDC.ShardingTest.Seed2.Maps;

public class StringEntityMap:IEntityTypeConfiguration<StringEntity>
{
    public void Configure(EntityTypeBuilder<StringEntity> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).IsRequired().HasMaxLength(128).HasComment("id");
        builder.Property(o => o.Column1).IsRequired().HasColumnType("char(128)").HasComment("column1");
        builder.Property(o => o.Column2).HasColumnType("char(127)").HasComment("column2");
        builder.Property(o => o.Column3).IsRequired().HasColumnType("varchar(128)").HasComment("column3");
        builder.Property(o => o.Column4).HasColumnType("varchar(127)").HasComment("column4");
        builder.Property(o => o.Column5).IsRequired().HasColumnType("tinytext").HasComment("column5");
        builder.Property(o => o.Column6).HasColumnType("tinytext").HasComment("column6");
        builder.Property(o => o.Column7).IsRequired().HasColumnType("text").HasComment("column7");
        builder.Property(o => o.Column8).HasColumnType("text").HasComment("column8");
        builder.Property(o => o.Column9).IsRequired().HasColumnType("mediumtext").HasComment("column9");
        builder.Property(o => o.Column10).HasColumnType("mediumtext").HasComment("column10");
        builder.ToTable("string_entity");
    }
}