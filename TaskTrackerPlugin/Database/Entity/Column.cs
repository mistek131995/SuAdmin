using System.ComponentModel.DataAnnotations;

namespace TaskTrackerPlugin.Database.Entity;

public class Column
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public int SequenceNumber { get; set; }
}