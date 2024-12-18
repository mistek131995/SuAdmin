using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PluginContracts;

public interface IPlugin
{
    public string Name { get; }
    Task CreateDatabase();
    void AddService(IServiceCollection services);
}