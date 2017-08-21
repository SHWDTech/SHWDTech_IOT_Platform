
// ReSharper disable InconsistentNaming

using System;
using ProtocolCommunicationService.NetWorkCore;
using SHWDTech.IOT.Storage.Communication.Entities;

namespace ProtocolCommunicationService.Core
{
    public class IOTDevice
    {
        private readonly Device _device;

        private DeviceClient _deviceClient;

        public string Name => _device.DeviceName;

        public string NodeIdHexString => _device.NodeIdHexString;

        public DateTime ConnectedTime { get; private set; }

        public DateTime DisconnectedTime { get; private set; }

        public DateTime HeartBeatTime { get; private set; }

        public DateTime ReceiveDataTime { get; private set; }

        public DateTime SendDataTime { get; private set; }

        public DateTime ProcessProtocolTime { get; private set; }

        public IOTDevice(Device dev)
        {
            _device = dev;
        }

        public void SetupClient(DeviceClient client)
        {
            _deviceClient?.Dispose();
            _deviceClient = client;
            _deviceClient.OnDataReceived += DataReceived;
            _deviceClient.OnDataSend += DataSend;
            _deviceClient.OnDisconnected += Disconnected;
        }

        public void UpdateConnectTime()
        {
            ConnectedTime = DateTime.Now;
        }

        public void UpdateDisconnectTime()
        {
            DisconnectedTime = DateTime.Now;
        }

        public void UpdateHeartBeatTime()
        {
            HeartBeatTime = DateTime.Now;
        }

        public void UpdateReceiveDataTime()
        {
            ReceiveDataTime = DateTime.Now;
        }

        public void UpdateSendDataTime()
        {
            SendDataTime = DateTime.Now;
        }

        public void UpdateProcessProtocolTime()
        {
            ProcessProtocolTime = DateTime.Now;
        }

        private void DataReceived(ClientDataReceivedEventArgs args)
        {
            UpdateReceiveDataTime();
        }

        private void DataSend(ClientSendDataEventArgs args)
        {
            UpdateSendDataTime();
        }

        private void Disconnected(ClientDisconnectedEventArgs args)
        {
            UpdateDisconnectTime();
            _deviceClient?.Dispose();
            _deviceClient = null;
        }
    }
}
