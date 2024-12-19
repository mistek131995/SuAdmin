using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaPlugin.Database;
using KafkaPlugin.Database.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KafkaPlugin;

public class Main : PluginContracts.IPlugin
{
    public string Name { get; } = "Kafka";
    public string Description { get; } = """
                                             Плагин Kafka предоставляет удобный интерфейс для просмотра данных в топиках, 
                                             управления ими, мониторинга состояния брокеров и работы с потребителями и продюсерами, 
                                             упрощая администрирование и анализ системы.
                                         """;


    public async Task InstallPlugin()
    {
        await CreateDatabase();
    }

    public void AddService(IServiceCollection services)
    {
        services.AddTransient<IAdminClient>(x =>
        {
            var adminClientConfig = new AdminClientConfig()
            {
                BootstrapServers = "localhost:9092"
            };

            return new AdminClientBuilder(adminClientConfig).Build();
        });
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
            
            context.Migrations.Add(new Migration()
            {
                Name = migrationName
            });
            await context.SaveChangesAsync();
        }
        
    }
}