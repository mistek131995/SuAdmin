using Microsoft.AspNetCore.Components;
using PluginContracts;
using SuAdmin.Extensions;


namespace SuAdmin.Components.Pages;

public partial class PluginManager : ComponentBase
{
    private List<IPlugin>? _plugins;

    protected override async Task OnInitializedAsync()
    {
        _plugins = AppDomain.CurrentDomain.GetPluginsMainInstanceFromAssembly();
    }
}