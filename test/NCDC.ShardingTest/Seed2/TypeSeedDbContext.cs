using Microsoft.EntityFrameworkCore;
using NCDC.ShardingTest.Seed2.Maps;

namespace NCDC.ShardingTest.Seed2;

public class TypeSeedDbContext:DbContext
{
    public TypeSeedDbContext(DbContextOptions<TypeSeedDbContext> options):base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new StringEntityMap());
        modelBuilder.ApplyConfiguration(new NumberEntityMap());
        modelBuilder.ApplyConfiguration(new DateTimeEntityMap());
    }
}