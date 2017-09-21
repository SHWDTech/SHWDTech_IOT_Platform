using System;
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

        public string NodeId { get; set; }

        public List<RechargeShot> RechargeShots { get; set; }

        private readonly Socket _client;

        private readonly byte[] _readBuffer;

        public ChargingPileClient(IPAddress address, int port)
        {
            _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _client.Bind(new IPEndPoint(address, port));
            _readBuffer = new byte[512];
        }

        public void Connected(IPAddress address, int port)
        {
            _client.Connect(address, port);
            Task.Factory.StartNew(() =>
            {
                while (_client.Connected)
                {
                    _client.Receive(_readBuffer);
                    ParseCommand();
                    Thread.Sleep(100);
                }
            });
        }

        public void Send(byte[] data)
        {
            _client.Send(data);
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
            if (_readBuffer[17] == 0x00)
            {
                SeftTestResponse();
            }
        }

        private void SeftTestResponse()
        {
            var response = new List<byte>();
            response.AddRange(_readBuffer);
            response.RemoveRange(23, _readBuffer.Length - 23);
            response[2] = 0x02;
            response[3] = 0x84;
            short length = 7;
            var lengthBytes = BitConverter.GetBytes(length);
            Array.Reverse(lengthBytes);
            response[14] = lengthBytes[0];
            response[15] = lengthBytes[1];
            response[22] = 0;
            var crc = GetCrcModBus(response.ToArray());
            var crcBytes = BitConverter.GetBytes(crc);
            Array.Reverse(crcBytes);
            response.AddRange(crcBytes);
            response.Add(0xAA);
            Send(response.ToArray());
        }

        public static ushort GetCrcModBus(byte[] buffer)
        {
            ushort regCrc = 0xffff;

            var len = buffer.Length;

            for (var i = 0; i < len; i++)
            {
                regCrc ^= buffer[i];
                for (var j = 0; j < 8; j++)
                {
                    if ((regCrc & 0x0001) != 0)

                        regCrc = (ushort)(regCrc >> 1 ^ 0xA001);
                    else
                        regCrc >>= 1;
                }
            }
            var tempReg = (byte)(regCrc >> 8);

            return (ushort)(regCrc << 8 | tempReg);
        }

        public static byte[] HeartBeatTemple = { 0x55, 0xF1, 0x06, 0x84, 0x00, 0x11, 0x08, 0x0E, 0x0C, 0x34, 0x18, 0x02, 0x37, 0x08, 0x00, 0x08 };

        public static byte[] HeartBeat(string nodeId)
        {
            var list = new List<byte>();
            list.AddRange(HeartBeatTemple);
            var nodeidArray = BitConverter.GetBytes(long.Parse(nodeId));
            Array.Reverse(nodeidArray);
            list.AddRange(nodeidArray);
            var crc = GetCrcModBus(list.ToArray());
            var crcBytes = BitConverter.GetBytes(crc);
            Array.Reverse(crcBytes);
            list.AddRange(crcBytes);
            list.Add(0xAA);
            return list.ToArray();
        }
    }

    public class RechargeShot
    {
        public string Identity { get; set; }
    }
}
