using Microsoft.EntityFrameworkCore;
using NCDC.EntityFrameworkCore.Maps;

namespace NCDC.EntityFrameworkCore;

public class NCDCDbContext:DbContext
{
    public NCDCDbContext(DbContextOptions<NCDCDbContext> options):base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new LogicDatabaseMap());
        modelBuilder.ApplyConfiguration(new DataSourceMap());
    }
}