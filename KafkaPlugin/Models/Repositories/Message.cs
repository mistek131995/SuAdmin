using System.Collections.Generic;

namespace KafkaPlugin.Models.Repositories;

public class Message
{
    public string Topic { get; set; }
    public string? Key { get; set; }
    public Dictionary<string, byte[]> Headers { get; set; }
    public string Payload { get; set; }
}