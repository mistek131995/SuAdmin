using System.Collections.Generic;
using System.Threading.Tasks;
using KafkaPlugin.Models.Repositories;
using KfkAdmin.Interfaces.Common;

namespace KafkaPlugin.Interfaces.Repositories;

public interface IMessageRepository : IBaseKafkaRepository
{
    Task<List<Message>> GetByTopicNameAsync(string topicName);
    Task SendMessagesAsync(Message messages);
}