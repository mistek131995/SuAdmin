using Microsoft.AspNetCore.Components;
using SuAdmin.Extensions;

namespace SuAdmin.Components.Pages.PluginManager;

public partial class PluginManager : ComponentBase
{
    [Parameter] public string? assemblyName { get; set; }

    private Type LoadComponent(string assemblyName)
    {
        return AppDomain.CurrentDomain.GetMainPageFromAssembly(assemblyName);
    }
}