using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PluginContracts;

public interface IPlugin
{
    void AddService(IServiceCollection services);
    void Configure(ModelBuilder modelBuilder);
}