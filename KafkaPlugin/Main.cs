using Confluent.Kafka;
using KafkaPlugin.Models.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KafkaPlugin;

public class Main : PluginContracts.IPlugin
{
    
    
    public void AddService(IServiceCollection services)
    {
        services.AddTransient<IAdminClient>(x =>
        {
            var adminClientConfig = new AdminClientConfig()
            {
                BootstrapServers = "localhost:9092"
            };

            return new AdminClientBuilder(adminClientConfig).Build();
        });
    }
    
    public void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<KafkaPluginSettings>(entity =>
        {
            entity.ToTable("KafkaPluginSettings");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Host).IsRequired().HasMaxLength(256);
            entity.Property(e => e.IsEnabled).IsRequired();
        });
    }
}