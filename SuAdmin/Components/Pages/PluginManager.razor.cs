using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using PluginContracts;
using SuAdmin.Extensions;
using SuAdmin.Infrastructure;
using SuAdmin.Infrastructure.Database;


namespace SuAdmin.Components.Pages;

public partial class PluginManager : ComponentBase
{
    [Inject] private SqlLiteContext _context { get; set; }
    
    private List<Plugin>? _installedPlugins;
    private List<IPlugin>? _plugins;

    protected override async Task OnInitializedAsync()
    {
        _installedPlugins = await _context.Plugins.ToListAsync();
        _plugins = AppDomain.CurrentDomain.GetPluginsMainInstanceFromAssembly();
    }

    private async Task InstallPlugin(IPlugin plugin)
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
            _plugins = AppDomain.CurrentDomain.GetPluginsMainInstanceFromAssembly();
            
            StateHasChanged();
        }
    }

    private async Task UninstallPlugin(IPlugin plugin)
    {
        var installedPlugin = await _context.Plugins.FirstOrDefaultAsync(x => x.Name == plugin.Name);

        if (installedPlugin != null)
        {
            _context.Plugins.Remove(installedPlugin);
            await _context.SaveChangesAsync();
            
            _installedPlugins = await _context.Plugins.ToListAsync();
            _plugins = AppDomain.CurrentDomain.GetPluginsMainInstanceFromAssembly();
        }
        
        StateHasChanged();
    }
}