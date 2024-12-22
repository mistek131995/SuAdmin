using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KafkaPlugin.Database;
using KafkaPlugin.Interfaces.Repositories;
using KafkaPlugin.Models.Repositories;
using KafkaPlugin.Utils;
using Microsoft.EntityFrameworkCore;

namespace KafkaPlugin.Service.Repositories;

public class BrokerRepository(Context context, KafkaClientBuilder kafkaClientBuilder) : IBrokerRepository
{
    public async Task<List<Broker>> GetAllAsync()
    {
        var allHosts = await context.Hosts.ToListAsync();
        var adminClient = await kafkaClientBuilder.Build();
        
        var brokers = new List<Broker>();
        
        var metadata = await Task.Run(() => adminClient.GetMetadata(TimeSpan.FromSeconds(5)));

        foreach (var host in allHosts)
        {
            brokers.Add(new Broker()
            {
                Host = host.Ip,
                Port = host.Port,
                IsAvailable = metadata.Brokers.Any(x => x.Host == host.Ip && x.Port == host.Port),
            });
        }
        
        return brokers;
    }

    public async Task<List<Broker>> GetByIdsAsync(IEnumerable<int> brokerIds)
    {
        var allHosts = await context.Hosts.ToListAsync();
        var adminClient = await kafkaClientBuilder.Build();
        
        var metadata = await Task.Run(() => adminClient.GetMetadata(TimeSpan.FromSeconds(10)));
        
        var brokers = new List<Broker>();
        
        foreach (var host in allHosts)
        {
            brokers.Add(new Broker()
            {
                Host = host.Ip,
                Port = host.Port,
                IsAvailable = metadata.Brokers.Any(x => x.Host == host.Ip && x.Port == host.Port),
            });
        }
        
        return brokers;
    }
}