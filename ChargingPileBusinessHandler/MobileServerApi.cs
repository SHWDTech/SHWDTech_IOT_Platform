using HttpRequest;

namespace ChargingPileBusiness
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


        public void GetChargingPileIdentityInformation(string nodeId, HttpResponseHandler handler)
        {
            var paramters = new XHttpRequestParamters();
            paramters.BodyParamters.Add("NodeId", nodeId);
            _requestClient.StartRequest(ApiAddrGetChargingPileInfo, HttpRequestClient.HttpMethodGet, paramters, handler);
        }
    }
}
