using System.Reflection;
using Microsoft.AspNetCore.Components;
using PluginContracts;
using SuAdmin.Models;

namespace SuAdmin.Extensions;

public static class Startup
{
    public static List<Plugin> LoadPlugins(this IServiceCollection services)
    {
        var pluginsMainFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");
        
        if (!Directory.Exists(pluginsMainFolderPath))
            Directory.CreateDirectory(pluginsMainFolderPath);

        var pluginFolders = Directory.GetDirectories(pluginsMainFolderPath).ToList();

        var plugins = new List<Plugin>();
        
        foreach (var pluginFolder in pluginFolders)
        {
            var pluginName = $"{Path.GetFileName(pluginFolder)}.dll"; ;
            
            var pluginDll = Directory.GetFiles(pluginFolder, pluginName).FirstOrDefault();
            var pluginAssembly = Assembly.LoadFrom(pluginDll);
            
            var type = pluginAssembly.GetTypes().FirstOrDefault(x => typeof(IPlugin).IsAssignableFrom(x) && !x.IsAbstract);
            var pluginInstance = (IPlugin) Activator.CreateInstance(type);
            pluginInstance.AddService(services);
                    
            var plugin = new Plugin
            {
                Name = pluginAssembly.GetName().Name,
                Version = pluginAssembly.GetName().Version.ToString(),
                Widgets = pluginAssembly.GetTypes().Where(t => t.BaseType.Name == typeof(ComponentBase).Name && !t.IsAbstract && t.FullName.Contains("Widget")).ToList(),
                MainPage = pluginAssembly.GetTypes().FirstOrDefault(t => t.BaseType.Name == typeof(ComponentBase).Name && !t.IsAbstract && t.FullName.Contains("MainPage"))
            };
                    
            plugins.Add(plugin);
            
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
        
        return plugins;
    }
}