using Microsoft.EntityFrameworkCore;

namespace NCDC.EntityFrameworkCore;

public class NCDCDbContext:DbContext
{
    public NCDCDbContext(DbContextOptions<NCDCDbContext> options):base(options)
    {
        
    }
}