using Microsoft.Extensions.DependencyInjection;

namespace PluginContracts;

public interface IPlugin
{
    void AddService(IServiceCollection services);
}