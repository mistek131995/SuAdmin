using System.IO;
using KafkaPlugin.Database.Database;
using Microsoft.EntityFrameworkCore;

namespace KafkaPlugin.Database;

internal class Context : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var dbSourcePath = Path.Combine("Plugins", typeof(Main).Assembly.GetName().Name, "KafkaPlugin.db");
            optionsBuilder.UseSqlite($"Data Source={dbSourcePath}");
        }
    }
    
    public DbSet<Migration> Migrations { get; set; }
    public DbSet<Host> Hosts { get; set; }
}