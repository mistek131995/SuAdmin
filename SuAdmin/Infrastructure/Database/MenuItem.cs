using System.ComponentModel.DataAnnotations;

namespace SuAdmin.Infrastructure.Database;

public class MenuItem
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Link { get; set; }
}