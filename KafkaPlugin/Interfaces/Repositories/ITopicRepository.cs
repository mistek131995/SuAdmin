using System.Collections.Generic;
using System.Threading.Tasks;
using KfkAdmin.Interfaces.Common;
using KfkAdmin.Models.Entities;

namespace KafkaPlugin.Interfaces.Repositories;

public interface ITopicRepository : IBaseKafkaRepository
{
    Task<List<Topic>> GetAllAsync();
    Task<List<Topic>> GetByBrokerIdAsync(int brokerId);
    Task<Topic?> GetByNameAsync(string name);
    
    Task CreateAsync(Topic topic);
    Task DeleteAsync(string name);
    Task DeleteAsync(List<string> names);
}