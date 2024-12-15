namespace PluginContracts;

public interface IPluginTable
{
    int Id { get; set; }
    bool IsEnabled { get; set; }
}