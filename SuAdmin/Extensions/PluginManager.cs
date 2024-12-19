using System.Reflection;
using Microsoft.AspNetCore.Components;
using PluginContracts;

namespace SuAdmin.Extensions;

public static class PluginManager
{
    public static async Task LoadPlugins(this IServiceCollection services)
    {
        var pluginsMainFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");
        
        if (!Directory.Exists(pluginsMainFolderPath))
            Directory.CreateDirectory(pluginsMainFolderPath);

        var pluginFolders = Directory.GetDirectories(pluginsMainFolderPath).ToList();
        
        foreach (var pluginFolder in pluginFolders)
        {
            var pluginName = $"{Path.GetFileName(pluginFolder)}.dll"; ;
            
            var pluginDll = Directory.GetFiles(pluginFolder, pluginName).FirstOrDefault();
            var pluginAssembly = Assembly.LoadFrom(pluginDll);
            
            var pluginInstance = pluginAssembly.GetPluginsMainInstanceFromAssembly();
            pluginInstance.AddService(services);
            
            var dependencyNames = pluginAssembly.GetReferencedAssemblies().Select(x => x.Name).ToList();
            var dependenciesDll = Directory
                .GetFiles(pluginFolder, "*.dll")
                .Where(x => !x.Contains(pluginName))
                .ToList();

            foreach (var file in dependenciesDll)
            {
                if(!dependencyNames.Contains(Path.GetFileNameWithoutExtension(file)))
                    continue;
                
                var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetName().Name);
                var assemblyName = AssemblyName.GetAssemblyName(file);
                
                if(loadedAssemblies.Contains(assemblyName.Name))
                    continue;
                
                Assembly.LoadFrom(file);
            }
        }
    }
    
    public static IPlugin GetPluginsMainInstanceFromAssembly(this Assembly assembly)
    {
        return assembly
            .GetTypes()
            .Where(x => typeof(IPlugin).IsAssignableFrom(x) && !x.IsAbstract)
            .Select(x => (IPlugin) Activator.CreateInstance(x))
            .FirstOrDefault();
    }
    
    public static List<IPlugin> GetPluginsMainInstanceFromAssembly(this AppDomain assembly)
    {
        return AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(x => x.GetTypes().Where(x => typeof(IPlugin).IsAssignableFrom(x) && !x.IsAbstract))
            .Select(x => (IPlugin) Activator.CreateInstance(x))
            .ToList();
    }
    
    public static List<Type> GetWidgetsInstanceFromAssembly(this AppDomain assembly)
    {
        return AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(x => x.GetTypes().Where(t => t.BaseType?.Name == nameof(ComponentBase) && !t.IsAbstract && t.FullName.Contains("Widget")).ToList())
            .ToList();
    }
}