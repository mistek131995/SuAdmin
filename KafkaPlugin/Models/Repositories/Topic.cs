using System.Collections.Generic;

namespace KafkaPlugin.Models.Repositories;

public class Topic
{
    public string Name { get; set; }
    public List<int> BrokerIds { get; set; }
    public int PartitionCount { get; set; }
    public short ReplicationFactor { get; set; }
    public long MessageCount { get; set; }
    public int MinOffset { get; set; }
    public int MaxOffset { get; set; }
}