using System;
using System.Collections.Generic;
using System.Net;
using ProtocolCommunicationService.Core;
// ReSharper disable All

namespace ProtocolCommunicationService
{
    public class ServiceControl
    {
        public static ServiceControl Instance { get; }

        static ServiceControl()
        {
            Instance = new ServiceControl();
        }

        private ServiceControl()
        {
            
        }

        public static string DbConnString { get; private set; }

        public static IPAddress ServerPublicIpAddress { get; private set; }

        private static bool _isInited;

        private readonly Dictionary<Guid, BusinessControl> _businessControls = new Dictionary<Guid, BusinessControl>();

        public static void Init(string dbConnString, IPAddress serveripAddress = null)
        {
            DbConnString = dbConnString;
            if (serveripAddress == null)
            {
                var addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
                if (addressList.Length <= 0)
                    throw new ArgumentException("Cann't fetch server ip address", nameof(ServerPublicIpAddress));
                foreach (var addr in addressList)
                {
                    var head = int.Parse(addr.ToString().Split('.')[0]);
                    if (head < 1 || head > 126) continue;
                    ServerPublicIpAddress = addr;
                    break;
                }
                if (ServerPublicIpAddress == null)
                    throw new ArgumentException("server don't have a external ip address",
                        nameof(ServerPublicIpAddress));
            }
            else
            {
                ServerPublicIpAddress = serveripAddress;
            }
            _isInited = true;
        }

        private static void CheckInit()
        {
            if (!_isInited) throw new InvalidOperationException("Must init first");
        }

        public bool StartBusiness(Guid bussnesssId)
        {
            CheckInit();
            BusinessControl control;
            if (_businessControls.ContainsKey(bussnesssId))
            {
                control = _businessControls[bussnesssId];
            }
            else
            {
                var business = BusinessLoader.LoadBusiness(bussnesssId);
                if (business == null) return false;
                control = new BusinessControl(business);
                _businessControls.Add(bussnesssId, control);
            }
            control.Start();
            return true;
        }

        public BusinessControl this[Guid businessId] => 
            !_businessControls.ContainsKey(businessId) 
            ? null 
            : _businessControls[businessId];
    }
}
