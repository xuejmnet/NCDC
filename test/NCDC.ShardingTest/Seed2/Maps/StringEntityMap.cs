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
        builder.Property(o => o.Column11).IsRequired().HasColumnType("longtext").HasComment("column11");
        builder.Property(o => o.Column12).HasColumnType("longtext").HasComment("column12");
        builder.Property(o => o.Column13).IsRequired().HasColumnType("tinyblob").HasComment("column13");
        builder.Property(o => o.Column14).HasColumnType("tinyblob").HasComment("column14");
        builder.Property(o => o.Column15).IsRequired().HasColumnType("blob").HasComment("column15");
        builder.Property(o => o.Column16).HasColumnType("blob").HasComment("column16");
        builder.Property(o => o.Column17).IsRequired().HasColumnType("mediumblob").HasComment("column17");
        builder.Property(o => o.Column18).HasColumnType("mediumblob").HasComment("column18");
        builder.Property(o => o.Column19).IsRequired().HasColumnType("longblob").HasComment("column19");
        builder.Property(o => o.Column20).HasColumnType("longblob").HasComment("column20");
       
        builder.ToTable("string_entity");
    }
}