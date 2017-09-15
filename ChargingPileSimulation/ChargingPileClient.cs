using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ChargingPileSimulation
{
    public class ChargingPileClient
    {
        public string Identity { get; set; }

        public List<RechargeShot> RechargeShots { get; set; }

        private readonly TcpClient _client;

        private NetworkStream _clientNetworkStream;

        private readonly byte[] _readBuffer;

        public ChargingPileClient(IPAddress address, int port)
        {
            _client = new TcpClient(new IPEndPoint(address, port));
            _readBuffer = new byte[512];
        }

        public void Connected(IPAddress address, int port)
        {
            _client.Connect(address, port);
            lock (_client)
            {
                _clientNetworkStream = _client.GetStream();
            }
            Task.Factory.StartNew(() =>
            {
                while (_client.Connected)
                {
                    lock (_client)
                    {
                        _clientNetworkStream.Read(_readBuffer, 0, _readBuffer.Length);
                    }
                    ParseCommand();
                    Thread.Sleep(100);
                }
            });
        }

        public void Send(byte[] data)
        {
            lock (_client)
            {
                _clientNetworkStream.Write(data, 0, data.Length);
            }
        }

        private void ParseCommand()
        {
            if (_readBuffer[0] == 0x55)
            {
                if (_readBuffer[1] == 0xF3)
                {
                    Response();
                }
            }
            for (var i = 0; i < _readBuffer.Length; i++)
            {
                _readBuffer[i] = 0x00;
            }
        }

        private void Response()
        {
            if (_readBuffer[15] == 0x06)
            {
                ChargResponse();
            }
        }

        private void ChargResponse()
        {
            
        }
    }

    public class RechargeShot
    {
        public string Identity { get; set; }
    }
}
