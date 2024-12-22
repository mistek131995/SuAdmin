namespace KafkaPlugin.Models.Repositories;

public class Broker
{
    public int BrokerId { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public bool IsAvailable { get; set; }
    
}