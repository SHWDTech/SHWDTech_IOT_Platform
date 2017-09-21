using System.Collections.Generic;
using ProtocolCommunicationService.Coding;
using ProtocolCommunicationService.NetWorkCore;
using SHWDTech.IOT.Storage.Communication.Entities;

namespace ProtocolCommunicationService.Core
{
    public class BusinessControl
    {
        private readonly DeviceListener _deviceListener;

        private readonly Dictionary<string, IOTDevice> _iotDevices = new Dictionary<string, IOTDevice>();

        public Business Business { get; }

        public bool IsListening => _deviceListener.IsListening;

        public BusinessControl(Business business)
        {
            Business = business;
            _deviceListener = new DeviceListener(Business);
            _deviceListener.OnClientConnected += ClientConnected;
            _deviceListener.OnClientDisconnected += ClientDisconnected;
            _deviceListener.OnClientAuthenticated += ClientAuthenticated;
            _deviceListener.OnClientPackageDecodeSuccessed += ClientPackageDecodeSuccessed;
        }

        public void Start()
        {
            _deviceListener.StartListen(ServiceControl.ServerPublicIpAddress, Business.Port);
        }

        private void ClientConnected(ClientConnectedEventArgs args)
        {
            ServiceControl.Instance.ClientConnected(args, this);
        }

        private void ClientDisconnected(ClientDisconnectedEventArgs args)
        {
            ServiceControl.Instance.ClientDisconnected(args, this);
        }

        private void ClientAuthenticated(ClientAuthenticatedArgs args)
        {
            ServiceControl.Instance.ClientAuthenticated(args, this);
        }

        private void ClientPackageDecodeSuccessed(ClientDecodeSucessEventArgs args)
        {
            EncoderManager.BusinessHandlers[Business.Id]?.OnPackageReceive(args.DecodedPackage);
        }

        public IOTDevice LookUpIotDevice(string nodeid)
        {
            if (string.IsNullOrWhiteSpace(nodeid)) return null;
            return _iotDevices.ContainsKey(nodeid) ? _iotDevices[nodeid] : null;
        }

        public void AppendIotDevice(IOTDevice device)
        {
            if (_iotDevices.ContainsKey(device.NodeIdString))
            {
                _iotDevices[device.NodeIdString] = device;
                return;
            }
            _iotDevices.Add(device.NodeIdString, device);
        }
    }
}
