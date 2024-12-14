using Confluent.Kafka;
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
}