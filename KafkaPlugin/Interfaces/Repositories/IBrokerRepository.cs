using System.Collections.Generic;
using System.Threading.Tasks;
using KafkaPlugin.Models.Repositories;
using KfkAdmin.Interfaces.Common;

namespace KafkaPlugin.Interfaces.Repositories;

public interface IBrokerRepository : IBaseKafkaRepository
{
    Task<List<Broker>> GetAllAsync();
    Task<List<Broker>> GetByIdsAsync(IEnumerable<int> brokerIds);
}