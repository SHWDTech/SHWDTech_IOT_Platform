using System;
using System.Net;

namespace ChargingPileSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("input local ipaddress");
            var address = IPAddress.Parse(Console.ReadLine());
            Console.WriteLine("input local port");
            var localPort = int.Parse(Console.ReadLine());
            var client = new ChargingPileClient(address, localPort);
            client.Connected(IPAddress.Parse("192.168.1.110"), 6993);
            client.Send(ChargingPileClient.HeartBeat("100000001"));
            Console.ReadKey();
        }
    }
}
