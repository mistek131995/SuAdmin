using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PluginContracts;

public interface IPlugin
{
    public string Name { get; }
    Task InstallPlugin();
    void AddService(IServiceCollection services);
}