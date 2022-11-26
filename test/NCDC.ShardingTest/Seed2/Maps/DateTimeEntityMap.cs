using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCDC.ShardingTest.Seed2.Entities;

namespace NCDC.ShardingTest.Seed2.Maps;

public class DateTimeEntityMap:IEntityTypeConfiguration<DateTimeEntity>
{
    public void Configure(EntityTypeBuilder<DateTimeEntity> builder)
    {
        
        
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).IsRequired().HasMaxLength(128).HasComment("id");
        builder.Property(o => o.Column1).IsRequired().HasColumnType("year").HasComment("column1");
        builder.Property(o => o.Column2).HasColumnType("year").HasComment("column2");
        builder.Property(o => o.Column3).IsRequired().HasColumnType("date").HasComment("column3");
        builder.Property(o => o.Column4).HasColumnType("date").HasComment("column4");
        builder.Property(o => o.Column5).IsRequired().HasColumnType("datetime").HasComment("column5");
        builder.Property(o => o.Column6).HasColumnType("datetime").HasComment("column6");
        builder.Property(o => o.Column7).IsRequired().HasColumnType("timestamp").HasComment("column7");
        builder.Property(o => o.Column8).HasColumnType("timestamp").HasComment("column8");
        builder.Property(o => o.Column9).IsRequired().HasColumnType("time").HasComment("column9");
        builder.Property(o => o.Column10).HasColumnType("time").HasComment("column10");
       
        builder.ToTable("datetime_entity");
    }
}