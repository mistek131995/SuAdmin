using Microsoft.Extensions.DependencyInjection;

namespace Contracts.Interfaces;

public interface IPlugin
{
    string Name { get; }
    string Description { get; }
    
    Task InstallPlugin();
    void AddService(IServiceCollection services);
}