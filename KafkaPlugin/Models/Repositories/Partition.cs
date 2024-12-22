namespace KafkaPlugin.Models.Repositories;

public class Partition
{
    public int Id { get; set; }
    public int BrokerId { get; set; }
    public long MinOffset { get; set; }
    public long MaxOffset { get; set; }
}