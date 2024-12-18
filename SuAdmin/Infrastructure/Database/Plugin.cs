using System.ComponentModel.DataAnnotations;

namespace SuAdmin.Infrastructure.Database;

public class Plugin
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Assembly { get; set; }
}