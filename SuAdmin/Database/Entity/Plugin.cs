using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuAdmin.Database.Entity;

[Table("Plugins")]
public class Plugins : IHostTable
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Assembly { get; set; }
}