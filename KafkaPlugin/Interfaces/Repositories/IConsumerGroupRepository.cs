using System.Collections.Generic;
using System.Threading.Tasks;
using KafkaPlugin.Models.Repositories;
using KfkAdmin.Interfaces.Common;

namespace KafkaPlugin.Interfaces.Repositories;

public interface IConsumerGroupRepository : IBaseKafkaRepository
{
    Task<List<ConsumerGroup>> GetAllAsync();
}