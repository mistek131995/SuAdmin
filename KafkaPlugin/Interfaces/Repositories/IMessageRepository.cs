using System.Collections.Generic;
using System.Threading.Tasks;
using KfkAdmin.Interfaces.Common;
using KfkAdmin.Models.Entities;

namespace KafkaPlugin.Interfaces.Repositories;

public interface IMessageRepository : IBaseKafkaRepository
{
    Task<List<Message>> GetByTopicNameAsync(string topicName);
    Task SendMessagesAsync(Message messages);
}