using Microsoft.EntityFrameworkCore;
using backend_stepkind.Models;

namespace backend_stepkind.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Tutorial> Tutorials => Set<Tutorial>();
}
