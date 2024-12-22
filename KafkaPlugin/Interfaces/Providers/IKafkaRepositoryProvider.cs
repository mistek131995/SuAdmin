using KafkaPlugin.Interfaces.Repositories;

namespace KafkaPlugin.Interfaces.Providers;

public interface IKafkaRepositoryProvider
{
    IBrokerRepository BrokerRepository { get; }
    ITopicRepository TopicRepository { get; }
    IPartitionRepository PartitionRepository { get; }
    IMessageRepository MessageRepository { get; }
    IConsumerGroupRepository ConsumerGroupRepository { get; }
}