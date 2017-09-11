using System;
using System.Threading.Tasks;
using HttpRequest;
using Newtonsoft.Json;

namespace WebApiTestConsole
{
    class Program
    {
        static void Main()
        {
            var ret = GetChargingPileIdentityInformation("100000001");
            Console.WriteLine($"on return {ret.Result}");
            Console.ReadKey();
        }

        public static Task<string> GetChargingPileIdentityInformation(string nodeId)
        {
            var paramters = new XHttpRequestParamters();
            paramters.BodyParamters.Add("nodeid", nodeId);
            return new HttpRequestClient("http://140.206.70.162:8092/").StartRequestAsync("json/facilityportlist.aspx", HttpRequestClient.HttpMethodPost, paramters);
        }
    }
}
