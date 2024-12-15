using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PluginContracts.Database;

namespace SuAdmin.Services.Utils;

public static class SqlBuilder
{
    public static async Task<string> BuildTableSql(Context context, string tableName, List<(string columnName, Type columnType)> columns)
    {
        var query = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}';";
        var result = await TableExistsAsync(context, tableName);
        
        if(!result)
            return CreateTable(tableName, columns);
        else
            return await UpdateTable(context, tableName, columns);
    }

    private static string CreateTable(string tableName, List<(string columnName, Type columnType)> columns)
    {
        var sql = new StringBuilder();
        sql.Append($"CREATE TABLE IF NOT EXISTS {tableName} (");

        foreach (var column in columns)
        {
            if (column.columnName.ToLower() == "id")
                sql.Append($"{column.columnName} INTEGER PRIMARY KEY AUTOINCREMENT ");
            else
                sql.Append($",{column.columnName} {CastType(column.columnType)} NOT NULL ");
        }
        
        sql.Append(" ); ");
        
        return sql.ToString();
    }

    private static async Task<string> UpdateTable(Context context, string tableName, List<(string columnName, Type columnType)> columns)
    {
        return string.Empty;
    }

    private static string CastType(Type type)
    {
        if (type == typeof(int) || type == typeof(bool))
        {
            return " INTEGER ";
        }else if (type == typeof(string))
        {
            return " TEXT ";
        }
        else
        {
            throw new Exception("Type not supported");
        }
    }
    
    private static async Task<bool> TableExistsAsync(Context context, string tableName)
    {
        var query = $"SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name=@tableName";

        await using var connection = context.Database.GetDbConnection();
        await connection.OpenAsync(); // Открываем соединение с базой данных

        await using var command = connection.CreateCommand();
        command.CommandText = query;

        // Добавляем параметр для защиты от SQL-инъекций
        var tableNameParam = command.CreateParameter();
        tableNameParam.ParameterName = "@tableName";
        tableNameParam.Value = tableName;
        command.Parameters.Add(tableNameParam);

        // Выполняем запрос
        var result = (long)await command.ExecuteScalarAsync();
        return result > 0; // Если результат > 0, таблица существует
    }
}