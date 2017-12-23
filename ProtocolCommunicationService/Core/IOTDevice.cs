
// ReSharper disable InconsistentNaming

using System;
using System.Linq;
using BasicUtility;
using ProtocolCommunicationService.Coding;
using ProtocolCommunicationService.NetWorkCore;
using SHWDTech.IOT.Storage.Communication;
using SHWDTech.IOT.Storage.Communication.Entities;

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
            DeviceClient.OnPackageDecodedSuccessed += DeviceClientOnOnPackageDecodedSuccessed;

            UpdateConnectTime();
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
            StorageSendProtocolData(args.SendContent.ToArray());
        }

        private ReceiveFeedback[] DeviceClientOnOnPackageDecodedSuccessed(ClientDecodeSucessEventArgs args)
        {
            UpdateProcessProtocolTime();
            StorageReceivedProtocolData(args.DecodedPackage);
            return null;
        }

        private void StorageReceivedProtocolData(IProtocolPackage package)
        {
            try
            {
                using (var ctx = new CommunicationProtocolDbContext())
                {
                    var data = new ProtocolData
                    {
                        BusinessId = DeviceClient.Business.Id,
                        Type = ProtocolDataType.Receive,
                        ContentLength = package.PackageByteLenth,
                        DecodeDateTime = package.FinalizeDateTime,
                        DeviceId = long.Parse(ClientSource.ClientIdentity),
                        ProtocolContent = package.GetBytes(),
                        ProtocolId = package.Protocol.Id,
                        UpdateDateTime = DateTime.Now
                    };
                    ctx.ProtocolDatas.Add(data);
                    ctx.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                LogService.Instance.Error("storage protocol data failed", ex);
            }
        }

        private void StorageSendProtocolData(byte[] sendContents)
        {
            try
            {
                using (var ctx = new CommunicationProtocolDbContext())
                {
                    var data = new ProtocolData
                    {
                        BusinessId = DeviceClient.Business.Id,
                        Type = ProtocolDataType.Send,
                        ContentLength = sendContents.Length,
                        DeviceId = long.Parse(ClientSource.ClientIdentity),
                        ProtocolContent = sendContents,
                        UpdateDateTime = DateTime.Now
                    };
                    ctx.ProtocolDatas.Add(data);
                    ctx.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                LogService.Instance.Error("storage protocol data failed", ex);
            }
        }

        private void Disconnected(ClientDisconnectedEventArgs args)
        {
            UpdateDisconnectTime();
            DeviceClient = null;
        }
    }
}
