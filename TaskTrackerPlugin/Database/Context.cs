using Microsoft.EntityFrameworkCore;
using TaskTrackerPlugin.Database.Entity;
using Task = System.Threading.Tasks.Task;

namespace TaskTrackerPlugin.Database;

public class Context : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var dbSourcePath = Path.Combine("Plugins", typeof(Main).Assembly.GetName().Name, "TaskTracker.db");
            optionsBuilder.UseSqlite($"Data Source={dbSourcePath}");
        }
    }
    
    public DbSet<Migration> Migrations { get; set; }
    public DbSet<Column> Columns { get; set; }
}