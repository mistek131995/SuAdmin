using System.ComponentModel.DataAnnotations;
using PluginContracts;

namespace KafkaPlugin.Models.Tables;

public class KafkaPluginSettings : IPluginTable
{
    [Key]
    public int Id { get; set; }
    [MaxLength(256)]
    public string Host { get; set; }
    public bool IsEnabled { get; set; }
}