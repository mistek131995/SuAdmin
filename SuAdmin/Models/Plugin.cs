namespace SuAdmin.Models;

public class Plugin
{
    public string Name { get; set; }
    public string Version { get; set; }
    public List<Type> Widgets { get; set; }
    public Type MainPage { get; set; }
}