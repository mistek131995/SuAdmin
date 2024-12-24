using System.Reflection;
using Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskTrackerPlugin.Database;
using MigrationModel = TaskTrackerPlugin.Database.Entity.Migration;

namespace TaskTrackerPlugin;

public class Main : IPlugin
{
    public string Name => "Task Tracker";
    public string Description => "Простая доска задач для совместной работы над проектом.";
    public async Task InstallPlugin()
    {
        await CreateDatabase();
    }

    public void AddService(IServiceCollection services)
    {

    }
    
    private async Task<bool> TableExistsAsync(Context context, string tableName)
    {
        var query = $"SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name=@tableName";

        await using var connection = context.Database.GetDbConnection();
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = query;

        var tableNameParam = command.CreateParameter();
        tableNameParam.ParameterName = "@tableName";
        tableNameParam.Value = tableName;
        command.Parameters.Add(tableNameParam);

        var result = (long)await command.ExecuteScalarAsync();
        return result > 0;
    }
    
    private async Task CreateDatabase()
    {
        await using var context = new Context(); 
        var migrationsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Plugins", Assembly.GetAssembly(typeof(Main)).GetName().Name, "Migrations");
        var migrationFiles = Directory.GetFiles(migrationsDirectory, "*.sql");

        for (var i = 0; i < migrationFiles.Length; i++)
        {
            var migrationFile = migrationFiles[i];
            var migrationName = Path.GetFileNameWithoutExtension(migrationFile);
            var query = await File.ReadAllTextAsync(migrationFile);

            if (i == 0)
            {
                var migrationsTableExists = await TableExistsAsync(context, "Migrations");

                if (!migrationsTableExists)
                { 
                    await context.Database.ExecuteSqlRawAsync(query);
                }
            }
            else
            {
                if (!await context.Migrations.AnyAsync(x => x.Name == migrationName))
                {
                    await context.Database.ExecuteSqlRawAsync(query);
                }
            }
            
            context.Migrations.Add(new MigrationModel()
            {
                Name = migrationName
            });
            await context.SaveChangesAsync();
        }
    }
}