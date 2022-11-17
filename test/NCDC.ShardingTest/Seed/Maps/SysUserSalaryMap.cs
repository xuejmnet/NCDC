using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCDC.ShardingTest.Seed.Entities;

namespace NCDC.ShardingTest.Seed.Maps
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 01 February 2021 15:42:35
* @Email: 326308290@qq.com
*/
    public class SysUserSalaryMap:IEntityTypeConfiguration<SysUserSalary>
    {
        public void Configure(EntityTypeBuilder<SysUserSalary> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).IsRequired().HasMaxLength(128);
            builder.Property(o => o.UserId).IsRequired().HasMaxLength(128);
            builder.ToTable(nameof(SysUserSalary));
        }
    }
}