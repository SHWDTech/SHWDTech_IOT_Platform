using System.Threading.Tasks;
using HttpRequest;

namespace SHWD.ChargingPileBusiness
{
    public class MobileServerApi
    {
        public static MobileServerApi Instance { get; }

        private readonly HttpRequestClient _requestClient;

        private MobileServerApi()
        {
            _requestClient = new HttpRequestClient("");
        }

        static MobileServerApi()
        {
            Instance = new MobileServerApi();
        }

        private const string ApiAddrGetChargingPileInfo = "";


        public Task<string> GetChargingPileIdentityInformation(string nodeId)
        {
            var paramters = new XHttpRequestParamters();
            paramters.BodyParamters.Add("nodeid", nodeId);
            return _requestClient.StartRequestAsync(ApiAddrGetChargingPileInfo, HttpRequestClient.HttpMethodGet, paramters);
        }
    }
}
