using System.ComponentModel.DataAnnotations.Schema;

namespace SuAdmin.Models;

[Table("Plugins")]
public class Plugin
{
    public string Name { get; set; }
    public string Version { get; set; }
    public List<Type> Widgets { get; set; }
    public Type MainPage { get; set; }
}