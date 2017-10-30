using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

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
            _localAddress = IPAddress.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
            Console.WriteLine("input local port");
            _localPort = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
        }

        private static void GetRemoteIpInfo()
        {
            Console.WriteLine("input remote ipaddress");
            _remoteAddress = IPAddress.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
            Console.WriteLine("input remote port");
            _remotePort = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
        }

        private static void PrepareClients()
        {
            _clients = new List<ChargingPileClient>();
            for (var i = 1; i < 20; i++)
            {
                var client = new ChargingPileClient(_localAddress, _localPort);
                _clients.Add(client);
                client.Identity = $"{100000000 + i}";
                _localPort++;
            }
        }

        private static void StartConnected()
        {
            foreach (var client in _clients)
            {
                client.Connected(_remoteAddress, _remotePort);
            }
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    foreach (var client in _clients)
                    {
                        var nodeId = $"{100000000 + _clients.IndexOf(client)}";
                        client.NodeId = nodeId;
                        client.Send(ChargingPileClient.HeartBeat(nodeId));
                    }
                    Thread.Sleep(60000);
                }
            });
        }
    }
}
