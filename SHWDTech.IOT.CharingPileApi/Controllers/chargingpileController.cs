using System.Threading.Tasks;
using System.Web.Http;

namespace SHWDTech.IOT.CharingPileApi.Controllers
{
    public class ChargingPileController : BasicApiController
    {
        [HttpGet]
        [Route("api/ChargingPile/{identityCode:string}/Status")]
        public async Task<IHttpActionResult> GetStatusAsync(string identityCode)
        {
            var status = await BusinessHandler.GetChargingPileStatus(identityCode);
            return Ok(status);
        }

        [HttpGet]
        [Route("api/ChargingPile/Status")]
        public async Task<IHttpActionResult> GetStatusAsync([FromBody]string[] identityCodes)
        {
            var status = await BusinessHandler.GetChargingPileStatus(identityCodes);
            return Ok(status);
        }
    }
}
