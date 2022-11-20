using Microsoft.EntityFrameworkCore;
using NCDC.EntityFrameworkCore.Maps;

namespace NCDC.EntityFrameworkCore;

public class NCDCDbContext:BaseNCDCDbContext<NCDCDbContext>
{
    public NCDCDbContext(DbContextOptions<NCDCDbContext> options):base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new LogicDatabaseMap());
        modelBuilder.ApplyConfiguration(new ActualDatabaseMap());
        modelBuilder.ApplyConfiguration(new LogicTableMap());
        modelBuilder.ApplyConfiguration(new AppAuthUserMap());
        modelBuilder.ApplyConfiguration(new ActualTableMap());
        modelBuilder.ApplyConfiguration(new LogicDatabaseUserMapMap());
    }
}