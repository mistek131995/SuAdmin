using System.Collections.Generic;
using System.Threading.Tasks;
using KafkaPlugin.Models.Repositories;
using KfkAdmin.Interfaces.Common;

namespace KafkaPlugin.Interfaces.Repositories;

public interface IPartitionRepository : IBaseKafkaRepository
{
    Task<List<Partition>> GetAllAsync();
    Task<List<Partition>> GetByTopicNameAsync(string name);
}