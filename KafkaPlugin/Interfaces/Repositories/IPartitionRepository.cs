using System.Collections.Generic;
using System.Threading.Tasks;
using KfkAdmin.Interfaces.Common;
using KfkAdmin.Models.Entities;

namespace KafkaPlugin.Interfaces.Repositories;

public interface IPartitionRepository : IBaseKafkaRepository
{
    Task<List<Partition>> GetAllAsync();
    Task<List<Partition>> GetByTopicNameAsync(string name);
}