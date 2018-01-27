using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.SelfHost;
using BasicUtility;
using ProtocolCommunicationService;
using ProtocolCommunicationService.Coding;
using SHWD.ChargingPileBusiness;
using SHWDTech.IOT.CharingPileApi.Controllers;
using SHWDTech.IOT.CharingPileApi.Providers;
using SHWDTech.IOT.Storage.Authorization.Repository;
using SHWDTech.IOT.Storage.Communication.Entities;
using SHWDTech.IOT.Storage.Communication.Repository;
using WebServerComponent.MessageHandler;

namespace ProtocolServiceCommandLine
{
    class Program
    {
        private static string _connStr;

        private static Business _business;

        private static string _serverIpAddress;

        private static void Main()
        {
            InputServerInfo();

            LoadChargingPileBusinessInformation();

            LoadConfiguration();

            HostApi();

            Console.ReadKey();
        }

        private static void InputServerInfo()
        {
            Console.WriteLine(@"input server ipaddress");
            _serverIpAddress = Console.ReadLine();
        }

        private static void LoadChargingPileBusinessInformation()
        {
            using (var repo = new CommunicationProtocolRepository())
            {
                _business = repo.FindBusinessByNameAsync("ChargingPile").Result;
                EncoderManager.LoadEncoder(repo.LoadProtocols());
            }
        }

        private static void LoadConfiguration()
        {
            _connStr = ConfigurationManager.AppSettings["CommunicationDbString"];
            if(string.IsNullOrWhiteSpace(_connStr)) throw new ArgumentException(nameof(_connStr));
            CommunicationProtocolRepository.ConnStr = _connStr;
            ServiceControl.Init(_connStr, IPAddress.Parse(_serverIpAddress));
            ServiceControl.Instance.StartBusiness(_business.Id);
            ServiceControl.Instance.OnClientConnected += (args, control) =>
            {
                var msg =
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} => client connected:" +
                    $" business:{control.Business.BusinessName}.clientIp:{args.TargetSocket.RemoteEndPoint}\r\n";
                Console.WriteLine(msg);
                LogService.Instance.Error(msg);
            };
            ServiceControl.Instance.OnClientDisconnected += (args, control) =>
            {
                var msg = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} => client disconnected: " +
                          $"business:{control.Business.BusinessName}.\r\nclientIp:{args.RemoteEndPoint}\r\n";
                Console.WriteLine(msg);
                LogService.Instance.Error(msg);
            };
            ServiceControl.Instance.OnClientAuthticated += (args, control) =>
            {
                var msg =
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} => client authenticated: business:{control.Business.BusinessName}." +
                    $"\r\nclientnodeid:{args.AuthenticatedClientSource.ClientNodeIdString}\r\n";
                Console.WriteLine(msg);
                LogService.Instance.Error(msg);
            };
            ServiceControl.Instance.OnClientSendData += (args, control) =>
            {
                var msg =
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} => client send data: business:{control.Business.BusinessName}." +
                    $"\r\nclientnodeid:{args.Client.ClientSource?.ClientNodeIdString}." +
                    $"\r\ncontent:{args.SendContent.Aggregate(string.Empty, (current, b) => current + b.ToString("X2"))}\r\n";
                //Console.WriteLine(msg);
                LogService.Instance.Error(msg);
            };
            ServiceControl.Instance.OnClientDecodeSuccessed += (args, control) =>
            {
                var msg =
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} => client received data: " +
                    $"business:{control.Business.BusinessName}.\r\nclientnodeid:{args.DecodedPackage?.ClientSource?.ClientNodeIdString}." +
                    $"\r\ncontent:{args.DecodedPackage.GetBytes().Aggregate(string.Empty, (current, b) => current + b.ToString("X2"))}\r\n";
                //Console.WriteLine(msg);
                LogService.Instance.Error(msg);
            };
            Console.WriteLine($@"tcp server started for business:{_business.BusinessName}");
        }

        private static void HostApi()
        {
            var config = new HttpSelfHostConfiguration($"http://{_serverIpAddress}:9090");

            var assembly = Assembly.GetExecutingAssembly().Location;
            var path = assembly.Substring(0, assembly.LastIndexOf("\\", StringComparison.Ordinal)) + "\\SHWDTech.IOT.CharingPileApi.dll";
            BasicApiController.RegisterBusinessHandler((ChargingPileBusinessHandler)EncoderManager.BusinessHandlers[_business.Id]);
            config.Services.Replace(typeof(IAssembliesResolver), new SelfHostAssemblyResolver(path));

            // Web API configuration and services
            var authenticationName = ConfigurationManager.AppSettings["AuthName"];
            if (authenticationName == null) throw new ArgumentException("lost application setting AuthName");
            using (var repo = new AuthRepository())
            {
                var schema = repo.FindServiceSchema(authenticationName);
                config.MessageHandlers.Add(new HmacAutheResponseDelegateHandler((ulong)schema.RequestMaxAgeInSeconds,
                    schema.ServiceSchemaName,
                    new ChargingPileAllowedAppProvider(schema.ServiceSchemaName)));
            }

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("API default", "api/{controller}/{id}", new {id = RouteParameter.Optional});
            var server = new HttpSelfHostServer(config);
            server.OpenAsync().Wait();
        }
    }
}

