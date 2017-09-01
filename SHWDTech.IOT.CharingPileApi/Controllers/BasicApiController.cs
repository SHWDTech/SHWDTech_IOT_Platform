using System.Web.Http;
using SHWD.ChargingPileBusiness;

namespace SHWDTech.IOT.CharingPileApi.Controllers
{
    public class BasicApiController : ApiController
    {
        public static ChargingPileBusinessHandler Instance { get; private set; }

        public static void RegisterBusinessHandler(ChargingPileBusinessHandler handler)
        {
            if (Instance != null) return;
            Instance = handler;
        }

        public ChargingPileBusinessHandler BusinessHandler => Instance;
    }
}
