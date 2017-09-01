using System;
using System.Net;
using ProtocolCommunicationService;

namespace WebApiTestConsole
{
    class Program
    {
        static void Main()
        {
            ServiceControl.Init("ChargingPile", IPAddress.Parse("192.168.1.110"));
            var businessId = Guid.Parse("17205af7-8ec4-11e7-962f-00163e0128b7");
            ServiceControl.Instance.StartBusiness(businessId);
            Console.ReadKey();
        }
    }
}
