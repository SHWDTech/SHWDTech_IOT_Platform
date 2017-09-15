using System.Threading.Tasks;
using HttpRequest;

namespace SHWD.ChargingPileBusiness
{
    public class MobileServerApi
    {
        private readonly HttpRequestClient _requestClient;

        public MobileServerApi(string serverAddress)
        {
            _requestClient = new HttpRequestClient(serverAddress);
        }

        private const string ApiAddrGetChargingPileInfo = "";

        public static string ResultTypeSeftTest => "0";

        public static string ResultTypeChargingStart => "1";

        public static string ResultTypeChargingStop => "2";

        public static string ResultTypeMachineFault => "3";

        public static string ResultTypeChargDatas => "4";


        public Task<string> GetChargingPileIdentityInformation(string nodeId)
        {
            var paramters = new XHttpRequestParamters();
            paramters.BodyParamters.Add("nodeid", nodeId);
            return _requestClient.StartRequestAsync(ApiAddrGetChargingPileInfo, HttpRequestClient.HttpMethodPost, paramters);
        }

        public Task<string> ControlResultReturn(string resulttype, string result, string identify)
        {
            var paramters = new XHttpRequestParamters();
            paramters.BodyParamters.Add("resulttype", resulttype);
            paramters.BodyParamters.Add("result", result);
            paramters.BodyParamters.Add("identify", identify);
            return _requestClient.StartRequestAsync(ApiAddrGetChargingPileInfo, HttpRequestClient.HttpMethodPost, paramters);
        }
    }
}
