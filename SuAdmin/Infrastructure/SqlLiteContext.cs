using Microsoft.EntityFrameworkCore;
using SuAdmin.Infrastructure.Database;

namespace SuAdmin.Infrastructure;

public class SqlLiteContext(DbContextOptions<SqlLiteContext> contextOptions) : DbContext(contextOptions)
{
    public DbSet<Plugin> Plugins { get; set; }
}