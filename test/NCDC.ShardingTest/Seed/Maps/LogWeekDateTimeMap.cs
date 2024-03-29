﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCDC.ShardingTest.Seed.Entities;

namespace NCDC.ShardingTest.Seed.Maps
{
    public class LogWeekDateTimeMap:IEntityTypeConfiguration<LogWeekDateTime>
    {
        public void Configure(EntityTypeBuilder<LogWeekDateTime> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).IsRequired().IsUnicode(false).HasMaxLength(50);
            builder.Property(o => o.Body).HasMaxLength(128);
            builder.ToTable(nameof(LogWeekDateTime));
        }
    }
}
