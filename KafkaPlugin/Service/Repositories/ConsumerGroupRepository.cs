using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka.Admin;
using KafkaPlugin.Interfaces.Repositories;
using KafkaPlugin.Models.Repositories;
using KafkaPlugin.Utils;

namespace KafkaPlugin.Service.Repositories;

public class ConsumerGroupRepository(KafkaClientBuilder kafkaClientBuilder) : IConsumerGroupRepository
{
    public async Task<List<ConsumerGroup>> GetAllAsync()
    {
        var adminClient = await kafkaClientBuilder.Build();
        
        var options = new ListConsumerGroupsOptions
        {
            RequestTimeout = TimeSpan.FromSeconds(2) // Указываем тайм-аут
        };
        var groups = await adminClient.ListConsumerGroupsAsync(options);

        return groups.Valid.Select(x => new ConsumerGroup()
        {
            GroupId = x.GroupId,
            State = x.State.ToString(),
            Type = x.Type.ToString(),
            IsSimpleConsumerGroup = x.IsSimpleConsumerGroup
        }).ToList();
    }
}