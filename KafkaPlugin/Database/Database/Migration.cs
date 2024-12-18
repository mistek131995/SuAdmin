using System.ComponentModel.DataAnnotations.Schema;

namespace KafkaPlugin.Database.Database;


public class Migration
{
    public int Id { get; set; }
    public string Name { get; set; }
}