using System.Collections.Generic;
using System.Threading.Tasks;
using KfkAdmin.Interfaces.Common;
using KfkAdmin.Models.Entities;

namespace KafkaPlugin.Interfaces.Repositories;

public interface IConsumerGroupRepository : IBaseKafkaRepository
{
    Task<List<ConsumerGroup>> GetAllAsync();
}