using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using PluginContracts;
using PluginContracts.Database;
using SuAdmin.Models;
using SuAdmin.Services.Utils;

namespace SuAdmin.Extensions;

public static class PluginManager
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
                Widgets = pluginAssembly.GetTypes().Where(t => t?.BaseType?.Name == nameof(ComponentBase) && !t.IsAbstract && t.FullName.Contains("Widget")).ToList(),
                MainPage = pluginAssembly.GetTypes().FirstOrDefault(t => t.BaseType?.Name == nameof(ComponentBase) && !t.IsAbstract && t.FullName.Contains("MainPage"))
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

    //TODO: Переименовать в конфигурирование DB
    public static async Task ConfigurePluginsDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        
        var mainTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => typeof(IPlugin).IsAssignableFrom(x) && !x.IsAbstract)
            .ToList();

        foreach (var type in mainTypes)
        {
            var pluginInstance = (IPlugin) Activator.CreateInstance(type);
            
            Context.DynamicModelBuilder = modelBuilder =>
            {
                pluginInstance.Configure(modelBuilder);
            };
        }
        
        var context = scope.ServiceProvider.GetRequiredService<Context>();
        await context.Database.EnsureCreatedAsync();

        var migrations = new StringBuilder();

        var tableTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => typeof(IPluginTable).IsAssignableFrom(x) && !x.IsAbstract).ToList();
        
        foreach (var tableType in tableTypes)
        {
            var tableName = tableType.Name;
            var tableColumns = tableType.GetProperties().Select(x => (x.Name, x.PropertyType)).ToList();
            
            //TODO: Добавить проверку на неизвестные типы
            
            migrations.Append(await SqlBuilder.BuildTableSql(context, tableName, tableColumns));
        }

        try
        {
            var sql = migrations.ToString();
            await context.Database.ExecuteSqlRawAsync(sql);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }
}