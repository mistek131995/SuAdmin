using System;
using System.Collections.Generic;
using KafkaPlugin.Interfaces.Providers;
using KafkaPlugin.Interfaces.Repositories;
using KafkaPlugin.Service.Repositories;
using KfkAdmin.Interfaces.Common;
using Microsoft.Extensions.DependencyInjection;

namespace KafkaPlugin.Service.Providers;

public class KafkaRepositoryProvider(IServiceProvider serviceProvider) : IKafkaRepositoryProvider
{
    private readonly Dictionary<Type, IBaseKafkaRepository> _repositories = new();
    
    private T Get<T>() where T : IBaseKafkaRepository
    {
        var type = typeof(T);

        if (_repositories.TryGetValue(type, out var repository))
        {
            return (T)repository;
        }

        repository = ActivatorUtilities.CreateInstance<T>(serviceProvider);

        _repositories[type] = repository;

        return (T)repository;
    }
    
    public IBrokerRepository BrokerRepository => Get<BrokerRepository>();
    public ITopicRepository TopicRepository => Get<TopicRepository>();
    public IPartitionRepository PartitionRepository => Get<PartitionRepository>();
    public IMessageRepository MessageRepository { get; }
    public IConsumerGroupRepository ConsumerGroupRepository => Get<ConsumerGroupRepository>();
}