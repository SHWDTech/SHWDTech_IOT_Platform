using System;
using System.Collections.Generic;
using System.Net;
using ProtocolCommunicationService.Core;

namespace ProtocolCommunicationService
{
    public class ServiceControl
    {
        public static string DbConnString { get; private set; }

        public static IPAddress ServerPublicIpAddress { get; private set; }

        private static bool _isInited;

        private static readonly Dictionary<Guid, BusinessControl> BusinessControls = new Dictionary<Guid, BusinessControl>();

        public static void Init(string dbConnString)
        {
            DbConnString = dbConnString;
            var addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            if (addressList.Length <= 0) throw new ArgumentException("Cann't fetch server ip address", nameof(ServerPublicIpAddress));
            foreach (var addr in addressList)
            {
                var head = int.Parse(addr.ToString().Split('.')[0]);
                if (head < 1 || head > 126) continue;
                ServerPublicIpAddress = addr;
                break;
            }
            if (ServerPublicIpAddress == null) throw new ArgumentException("server don't have a external ip address", nameof(ServerPublicIpAddress));
            _isInited = true;
        }

        private static void CheckInit()
        {
            if (_isInited) throw new InvalidOperationException("Must init first");
        }

        public static void StartBusiness(Guid bussnesssId)
        {
            CheckInit();
            BusinessControl control;
            if (BusinessControls.ContainsKey(bussnesssId))
            {
                control = BusinessControls[bussnesssId];
            }
            else
            {
                var business = BusinessLoader.LoadBusiness(bussnesssId);
                if (business == null) return;
                control = new BusinessControl(business);
                BusinessControls.Add(bussnesssId, control);
            }
            control.Start();
        }
    }
}
