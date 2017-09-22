using System;
using System.Collections.Generic;
using System.Net;

namespace ChargingPileSimulation
{
    class Program
    {
        private static List<ChargingPileClient> _clients;

        private static IPAddress _localAddress;

        private static int _localPort;

        private static IPAddress _remoteAddress;

        private static int _remotePort;

        static void Main(string[] args)
        {
            GetLocalIpInfo();

            PrepareClients();

            GetRemoteIpInfo();

            Console.WriteLine("wait press any key to start");

            StartConnected();

            Console.ReadKey();
            Console.WriteLine("OK");
            Console.ReadKey();
        }

        private static void GetLocalIpInfo()
        {
            Console.WriteLine("input local ipaddress");
            _localAddress = IPAddress.Parse(Console.ReadLine());
            Console.WriteLine("input local port");
            _localPort = int.Parse(Console.ReadLine());
        }

        private static void GetRemoteIpInfo()
        {
            Console.WriteLine("input remote ipaddress");
            _remoteAddress = IPAddress.Parse(Console.ReadLine());
            Console.WriteLine("input remote port");
            _remotePort = int.Parse(Console.ReadLine());
        }

        private static void PrepareClients()
        {
            _clients = new List<ChargingPileClient>();
            for (var i = 1; i < 8; i++)
            {
                _clients.Add(new ChargingPileClient(_localAddress, _localPort));
                _localPort++;
            }
        }

        private static void StartConnected()
        {
            foreach (var client in _clients)
            {
                client.Connected(_remoteAddress, _remotePort);
                client.Send(ChargingPileClient.HeartBeat($"{100000000 + _clients.IndexOf(client)}"));
            }
        }
    }
}
