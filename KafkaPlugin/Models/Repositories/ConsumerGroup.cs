namespace KafkaPlugin.Models.Repositories;

public class ConsumerGroup
{
    public string GroupId { get; set; }
    public string State { get; set; }
    public string Type { get; set; }
    public bool IsSimpleConsumerGroup  { get; set; }
    
}