using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace KafkaPlugin.Utils;

public class KafkaUtils
{
    public static async Task<bool> IsKafkaHostAvailableAsync(string ip, int port)
    {
        try
        {
            using var client = new TcpClient();
            var connectTask = client.ConnectAsync(ip, port);
            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(1));

            var completedTask = await Task.WhenAny(connectTask, timeoutTask);
            return completedTask == connectTask && client.Connected;
        }
        catch
        {
            return false;
        }
    }
}