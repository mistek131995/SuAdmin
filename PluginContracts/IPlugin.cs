using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PluginContracts;

public interface IPlugin
{
    string Name { get; }
    string Description { get; }
    
    Task InstallPlugin();
    void AddService(IServiceCollection services);
}