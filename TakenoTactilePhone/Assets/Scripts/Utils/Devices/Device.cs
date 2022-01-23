using System;
using System.Net;
using System.Net.Sockets;

public class Device
{
    
    public static string GetIPv4()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList) {
            if (IPAddress.IsLoopback(ip))
                continue;
            if (ip.AddressFamily == AddressFamily.InterNetwork) {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
    
}