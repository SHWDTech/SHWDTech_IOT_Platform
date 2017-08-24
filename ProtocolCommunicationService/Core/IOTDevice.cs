
// ReSharper disable InconsistentNaming

using System;
using ProtocolCommunicationService.NetWorkCore;
using SHWDTech.IOT.Storage.Communication.Entities;

namespace ProtocolCommunicationService.Core
{
    public class IOTDevice
    {
        public Device Device { get; }

        public DeviceClient DeviceClient { get; private set; }

        public string Name => Device.DeviceName;

        public string NodeIdHexString => Device.NodeIdHexString;

        public string NodeIdString => Device.NodeIdString;

        public DateTime ConnectedTime { get; private set; }

        public DateTime DisconnectedTime { get; private set; }

        public DateTime HeartBeatTime { get; private set; }

        public DateTime ReceiveDataTime { get; private set; }

        public DateTime SendDataTime { get; private set; }

        public DateTime ProcessProtocolTime { get; private set; }

        private IOTDevice(Device dev)
        {
            Device = dev;
        }

        public static IOTDevice ResolveIotDevice(Device dev)
        {
            return new IOTDevice(dev);
        }

        public void SetupClient(DeviceClient client)
        {
            DeviceClient?.Dispose();
            DeviceClient = client;
            DeviceClient.OnDataReceived += DataReceived;
            DeviceClient.OnDataSend += DataSend;
            DeviceClient.OnDisconnected += Disconnected;
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
            DeviceClient?.Dispose();
            DeviceClient = null;
        }
    }
}
