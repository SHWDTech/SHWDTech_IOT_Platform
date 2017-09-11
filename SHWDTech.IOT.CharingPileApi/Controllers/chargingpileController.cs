using System.Threading.Tasks;
using System.Web.Http;

namespace SHWDTech.IOT.CharingPileApi.Controllers
{
    public class ChargingPileController : BasicApiController
    {
        [HttpGet]
        [Route("api/ChargingPile/{identityCode}/Status")]
        public async Task<IHttpActionResult> GetStatusAsync(string identityCode)
        {
            var status = await BusinessHandler.GetChargingPileStatusAsync(identityCode);
            return Ok(status);
        }

        [HttpGet]
        [Route("api/ChargingPile/Status")]
        public async Task<IHttpActionResult> GetStatusAsync([FromBody]string[] identityCodes)
        {
            var status = await BusinessHandler.GetChargingPileStatusAsync(identityCodes);
            return Ok(status);
        }
    }
}
