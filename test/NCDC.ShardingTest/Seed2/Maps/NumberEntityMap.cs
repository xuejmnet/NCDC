using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCDC.ShardingTest.Seed2.Entities;

namespace NCDC.ShardingTest.Seed2.Maps;

public class NumberEntityMap:IEntityTypeConfiguration<NumberEntity>
{
    public void Configure(EntityTypeBuilder<NumberEntity> builder)
    {
        
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).IsRequired().HasMaxLength(128).HasComment("id");
        builder.Property(o => o.Column1).IsRequired().HasColumnType("tinyint").HasComment("column1");
        builder.Property(o => o.Column2).HasColumnType("tinyint").HasComment("column2");
        builder.Property(o => o.Column3).IsRequired().HasColumnType("tinyint").HasComment("column3");
        builder.Property(o => o.Column4).HasColumnType("tinyint").HasComment("column4");
        builder.Property(o => o.Column5).IsRequired().HasColumnType("smallint").HasComment("column5");
        builder.Property(o => o.Column6).HasColumnType("smallint").HasComment("column6");
        builder.Property(o => o.Column7).IsRequired().HasColumnType("smallint").HasComment("column7");
        builder.Property(o => o.Column8).HasColumnType("smallint").HasComment("column8");
        builder.Property(o => o.Column9).IsRequired().HasColumnType("int").HasComment("column9");
        builder.Property(o => o.Column10).HasColumnType("int").HasComment("column10");
        builder.Property(o => o.Column11).IsRequired().HasColumnType("int").HasComment("column11");
        builder.Property(o => o.Column12).HasColumnType("int").HasComment("column12");
        builder.Property(o => o.Column13).IsRequired().HasColumnType("bigint").HasComment("column13");
        builder.Property(o => o.Column14).HasColumnType("bigint").HasComment("column14");
        builder.Property(o => o.Column15).IsRequired().HasColumnType("bigint").HasComment("column15");
        builder.Property(o => o.Column16).HasColumnType("bigint").HasComment("column16");
        builder.Property(o => o.Column17).IsRequired().HasColumnType("float").HasComment("column17");
        builder.Property(o => o.Column18).HasColumnType("float").HasComment("column18");
        builder.Property(o => o.Column19).IsRequired().HasColumnType("double").HasComment("column19");
        builder.Property(o => o.Column20).HasColumnType("double").HasComment("column20");
        builder.Property(o => o.Column21).IsRequired().HasColumnType("decimal(18,6)").HasComment("column21");
        builder.Property(o => o.Column22).HasColumnType("decimal(18,6)").HasComment("column22");
        builder.Property(o => o.Column23).IsRequired().HasColumnType("tinyint(1)").HasComment("column23");
        builder.Property(o => o.Column24).HasColumnType("tinyint(1)").HasComment("column24");
       
        builder.ToTable("number_entity");
    }
}