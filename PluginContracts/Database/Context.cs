using Microsoft.EntityFrameworkCore;

namespace PluginContracts.Database;

public class Context(DbContextOptions<Context> contextOptions) : DbContext(contextOptions)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        DynamicModelBuilder?.Invoke(modelBuilder);
    }
    
    public static Action<ModelBuilder> DynamicModelBuilder { get; set; }
}