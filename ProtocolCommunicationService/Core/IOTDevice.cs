
// ReSharper disable InconsistentNaming

using System;
using ProtocolCommunicationService.Coding;
using ProtocolCommunicationService.NetWorkCore;

namespace ProtocolCommunicationService.Core
{
    public class IOTDevice
    {
        public IClientSource ClientSource { get; }

        public DeviceClient DeviceClient { get; private set; }

        public string Name => ClientSource.ClientIdentity;

        public string NodeIdString => ClientSource.ClientNodeIdString;

        public DateTime ConnectedTime { get; private set; }

        public DateTime DisconnectedTime { get; private set; }

        public DateTime HeartBeatTime { get; private set; }

        public DateTime ReceiveDataTime { get; private set; }

        public DateTime SendDataTime { get; private set; }

        public DateTime ProcessProtocolTime { get; private set; }

        private IOTDevice(IClientSource source)
        {
            ClientSource = source;
        }

        public static IOTDevice ResolveIotDevice(IClientSource source)
        {
            return new IOTDevice(source);
        }

        public void SetupClient(DeviceClient client)
        {
            DeviceClient?.Dispose();
            DeviceClient = client;
            client.ClientSource = ClientSource;
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
