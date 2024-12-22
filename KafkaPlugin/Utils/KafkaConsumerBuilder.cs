using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaPlugin.Database;
using Microsoft.EntityFrameworkCore;

namespace KafkaPlugin.Utils;

public class KafkaConsumerBuilder(Context context)
{
    public const string CONSUMER_GROUP_ID = "kfk-admin-group";
    
    private ConsumerBuilder<string?, string> _builder;

    private async Task AddHostsAsync()
    {
        var allHosts = await context.Hosts.ToListAsync();
        var availableHosts = await Task.WhenAll(allHosts.Select(async host =>
        {
            var isAvailable = await KafkaUtils.IsKafkaHostAvailableAsync(host.Ip, host.Port);
            return new { Host = host, IsAvailable = isAvailable };
        }));

        var filteredHosts = availableHosts
            .Where(host => host.IsAvailable)
            .Select(x => x.Host)
            .ToList();
        
        _builder = new ConsumerBuilder<string?, string>(new ConsumerConfig()
        {
            BootstrapServers = string.Join(";", filteredHosts.Select(x => $"{x.Ip}:{x.Port}")),
            GroupId = CONSUMER_GROUP_ID,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false,
            MetadataMaxAgeMs = 1000
        });
    }

    public async Task<IConsumer<string?, string>> Build()
    {
        await AddHostsAsync();
        
        return _builder.Build();
    }
}