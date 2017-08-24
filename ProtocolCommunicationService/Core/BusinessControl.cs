using System.Collections.Generic;
using ProtocolCommunicationService.NetWorkCore;
using SHWDTech.IOT.Storage.Communication.Entities;

namespace ProtocolCommunicationService.Core
{
    public class BusinessControl
    {
        private readonly DeviceListener _deviceListener;

        private readonly Dictionary<string, IOTDevice> _iotDevices = new Dictionary<string, IOTDevice>();

        public Business Business { get; }

        public BusinessControl(Business business)
        {
            Business = business;
            _deviceListener = new DeviceListener(Business);
            _deviceListener.OnClientConnected += ClientConnected;
        }

        public void Start()
        {
            _deviceListener.StartListen(ServiceControl.ServerPublicIpAddress, Business.Port);
        }

        private void ClientConnected(ClientConnectedEventArgs args)
        {
            
        }

        public IOTDevice LookUpIotDevice(byte[] nodeid)
        {
            long number = 0;
            for (var i = 0; i < nodeid.Length; i++)
            {
                number |= (long)nodeid[i] << ((nodeid.Length - i - 1) * 8);
            }
            var nodeidStr = $"{number}";
            if (string.IsNullOrWhiteSpace(nodeidStr)) return null;
            return _iotDevices.ContainsKey(nodeidStr) ? _iotDevices[nodeidStr] : null;
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
