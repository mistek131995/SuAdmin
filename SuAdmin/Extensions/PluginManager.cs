using System.Collections.Concurrent;
using System.Reflection;
using Contracts.Interfaces;
using Microsoft.AspNetCore.Components;

namespace SuAdmin.Extensions;

public static class PluginManager
{
    public static ConcurrentBag<Type> WidgetTypes { get; set; } = new();
    
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

            AddWidgets(pluginAssembly);
            
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

    /// <summary>
    /// Добавляет список виджетов и кэширует его
    /// </summary>
    /// <param name="assembly"></param>
    private static void AddWidgets(Assembly assembly)
    {
        var widgetTypes = assembly
            .GetTypes()
            .Where(t => t.BaseType?.Name == nameof(ComponentBase) && !t.IsAbstract && t.FullName.Contains("MainWidget"))
            .ToList();

        foreach (var widgetType in widgetTypes)
        {
            WidgetTypes.Add(widgetType);
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
        return assembly
            .GetAssemblies()
            .SelectMany(x => x.GetTypes().Where(x => typeof(IPlugin).IsAssignableFrom(x) && !x.IsAbstract))
            .Select(x => (IPlugin) Activator.CreateInstance(x))
            .ToList();
    }
    
    public static Type GetMainPageFromAssembly(this AppDomain assembly, string assemblyName)
    {
        var targetAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(x => x.GetName().Name == assemblyName);
        
        return targetAssembly?.GetTypes()
            .FirstOrDefault(t => typeof(ComponentBase).IsAssignableFrom(t) && !t.IsAbstract && t.Name == "MainPage");
    }
}