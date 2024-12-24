using System.ComponentModel.DataAnnotations;

namespace TaskTrackerPlugin.Database.Entity;

public class Migration
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
}