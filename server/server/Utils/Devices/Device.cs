using System.Net;
using System.Net.Sockets;

namespace server.Utils.Devices;

public class Device
{
    
    public static string GetIPv4()
    {
        var localIp = IPAddress.None;
        try
        {
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                // Connect socket to Google's Public DNS service
                socket.Connect("8.8.8.8", 65530);
                if (!(socket.LocalEndPoint is IPEndPoint endPoint))
                {
                    throw new InvalidOperationException($"Error occurred casting {socket.LocalEndPoint} to IPEndPoint");
                }
                localIp = endPoint.Address;
                socket.Dispose();
            }
        }
        catch (SocketException ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
        return localIp.ToString();
    }
    
}