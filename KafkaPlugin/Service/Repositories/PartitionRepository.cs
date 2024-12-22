﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaPlugin.Interfaces.Repositories;
using KafkaPlugin.Utils;
using Partition = KafkaPlugin.Models.Repositories.Partition;

namespace KafkaPlugin.Service.Repositories;

public class PartitionRepository(KafkaClientBuilder kafkaClientBuilder, KafkaConsumerBuilder kafkaConsumerBuilder) : IPartitionRepository
{
    public async Task<List<Partition>> GetAllAsync()
    {
        var adminClient = await kafkaClientBuilder.Build();
        var consumer = await kafkaConsumerBuilder.Build();
        
        var metadata = await Task.Run(() => adminClient.GetMetadata(TimeSpan.FromSeconds(10)));

        var partitions = new List<Partition>();

        foreach (var topic in metadata.Topics)
        {
            foreach (var partition in topic.Partitions)
            {
                var offsets = await Task.Run(() => consumer.QueryWatermarkOffsets(new TopicPartition(topic.Topic, partition.PartitionId), TimeSpan.FromSeconds(10)));

                partitions.Add(new Partition()
                {
                    Id = partition.PartitionId,
                    BrokerId = partition.Leader,
                    MinOffset = offsets?.Low.Value ?? 0,
                    MaxOffset = offsets?.High.Value ?? 0,
                });
            }
        }

        return partitions;
    }

    public async Task<List<Partition>> GetByTopicNameAsync(string name)
    {
        var adminClient = await kafkaClientBuilder.Build();
        var consumer = await kafkaConsumerBuilder.Build();
        
        var metadata = await Task.Run(() => adminClient.GetMetadata(name, TimeSpan.FromSeconds(10)));
        
        var partitions = new List<Partition>();

        foreach (var partition in metadata.Topics[0].Partitions)
        {
            var offsets = await Task.Run(() => consumer.QueryWatermarkOffsets(new TopicPartition(metadata.Topics[0].Topic, partition.PartitionId), TimeSpan.FromSeconds(10)));

            partitions.Add(new Partition()
            {
                Id = partition.PartitionId,
                BrokerId = partition.Leader,
                MinOffset = offsets?.Low.Value ?? 0,
                MaxOffset = offsets?.High.Value ?? 0,
            });
        }

        return partitions;
    }
}