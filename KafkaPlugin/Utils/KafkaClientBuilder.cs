using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaPlugin.Database;
using Microsoft.EntityFrameworkCore;

namespace KafkaPlugin.Utils;

public class KafkaClientBuilder(Context context)
{
    private IAdminClient _adminClient;

    public async Task<KafkaClientBuilder> AddHostsAsync()
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
        
        _adminClient = new AdminClientBuilder(new AdminClientConfig()
        {
            BootstrapServers = string.Join(";", filteredHosts.Select(x => $"{x.Ip}:{x.Port}"))
        }).Build();

        return this;
    }

    public async Task<IAdminClient> Build()
    {
        await AddHostsAsync();
        
        return _adminClient;
    }
}