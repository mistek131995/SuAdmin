using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using SuAdmin.Extensions;
using SuAdmin.Infrastructure;
using SuAdmin.Infrastructure.Database;

namespace SuAdmin.Components.Pages.PluginManager.Components;

public partial class PluginsTable : ComponentBase
{
    [Inject] private SqlLiteContext _context { get; set; }
    
    private List<Plugin>? _installedPlugins;
    private List<PluginViewModel>? _plugins;

    protected override async Task OnInitializedAsync()
    {
        _installedPlugins = await _context.Plugins.ToListAsync();
        _plugins = GetPluginsViewModels();
    }

    private async Task InstallPlugin(PluginViewModel plugin)
    {
        await plugin.InstallPlugin();

        if (!await _context.Plugins.AnyAsync(x => x.Name == plugin.Name))
        {
            await plugin.InstallPlugin();
            
            _context.Plugins.Add(new Plugin
            {
                Name = plugin.Name,
                Assembly = plugin.GetType().Assembly.GetName().Name,
                Version = plugin.GetType().Assembly.GetName().Version.ToString()
            });
            await _context.SaveChangesAsync();
    
            _installedPlugins = await _context.Plugins.ToListAsync();
            _plugins = GetPluginsViewModels();
            
            StateHasChanged();
        }
    }

    private async Task UninstallPlugin(PluginViewModel plugin)
    {
        var installedPlugin = await _context.Plugins.FirstOrDefaultAsync(x => x.Name == plugin.Name);

        if (installedPlugin != null)
        {
            _context.Plugins.Remove(installedPlugin);
            await _context.SaveChangesAsync();
            
            _installedPlugins = await _context.Plugins.ToListAsync();
            _plugins = GetPluginsViewModels();
        }
        
        StateHasChanged();
    }

    private List<PluginViewModel> GetPluginsViewModels()
    {
        var plugins = AppDomain.CurrentDomain.GetPluginsMainInstanceFromAssembly();

        return plugins.Select(x => new PluginViewModel()
        {
            Name = x.Name,
            Assembly = x.GetType().Assembly.GetName().Name,
            Version = x.GetType().Assembly.GetName().Version.ToString(),
            Description = x.Description,
            InstallPlugin = async () => await x.InstallPlugin()
        }).ToList();
    }

    private class PluginViewModel
    {
        public string Name { get; set; }
        public string Assembly { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public Func<Task> InstallPlugin { get; set; }
    }
}