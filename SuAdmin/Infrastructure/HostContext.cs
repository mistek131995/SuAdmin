using Microsoft.EntityFrameworkCore;
using SuAdmin.Database.Entity;

namespace SuAdmin.Infrastructure;

public class HostContext(DbContextOptions<HostContext> contextOptions) : DbContext(contextOptions)
{
    public DbSet<Plugins> Plugins { get; set; }
}