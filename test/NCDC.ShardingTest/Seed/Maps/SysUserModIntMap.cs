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
    public class SysUserModIntMap:IEntityTypeConfiguration<SysUserModInt>
    {
        public void Configure(EntityTypeBuilder<SysUserModInt> builder)
        {

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedNever();
            builder.Property(o => o.Name).HasMaxLength(128);
            builder.ToTable(nameof(SysUserModInt));
        }
    }
}
