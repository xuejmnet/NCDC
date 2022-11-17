﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCDC.ShardingTest.Seed.Entities;

namespace NCDC.ShardingTest.Seed.Maps
{
    public class LogYearDateTimeMap : IEntityTypeConfiguration<LogYearDateTime>
    {
        public void Configure(EntityTypeBuilder<LogYearDateTime> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.LogBody).IsRequired().HasMaxLength(256);
            builder.ToTable(nameof(LogYearDateTime));
        }
    }
}
