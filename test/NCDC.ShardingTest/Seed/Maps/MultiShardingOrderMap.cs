﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCDC.ShardingTest.Seed.Entities;

namespace NCDC.ShardingTest.Seed.Maps
{
    public class MultiShardingOrderMap:IEntityTypeConfiguration<MultiShardingOrder>
    {
        public void Configure(EntityTypeBuilder<MultiShardingOrder> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedNever();
            builder.Property(o => o.Name).IsRequired().IsUnicode(false).HasMaxLength(50);
            builder.ToTable(nameof(MultiShardingOrder));
        }
    }
}
