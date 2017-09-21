using System.Collections.Generic;
using ProtocolCommunicationService.NetWorkCore;

namespace ProtocolCommunicationService.Core
{
    public class ConnectedClients
    {
        public static ConnectedClients Instance { get; }

        private readonly List<DeviceClient> _connectedDeviceClients = new List<DeviceClient>();

        static ConnectedClients()
        {
            Instance = new ConnectedClients();
        }

        private ConnectedClients()
        {
            
        }

        public void Append(DeviceClient client)
        {
            client.OnAuthenticated += HandlerDeviceClientAuthenticated;
            _connectedDeviceClients.Add(client);
        }

        public void Remove(DeviceClient client)
        {
            if (_connectedDeviceClients.Contains(client))
            {
                _connectedDeviceClients.Remove(client);
            }
        }

        private static void HandlerDeviceClientAuthenticated(ClientAuthenticatedArgs args)
        {
            var control = ServiceControl.Instance[args.Business.Id];
            if (control == null) return;
            var iotDevice = control.LookUpIotDevice(args.AuthenticatedClientSource.ClientNodeIdString);
            if (iotDevice == null)
            {
                iotDevice = IOTDevice.ResolveIotDevice(args.AuthenticatedClientSource);
                control.AppendIotDevice(iotDevice);
            }
            iotDevice.SetupClient(args.Client);
        }
    }
}
