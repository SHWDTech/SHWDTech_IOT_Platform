using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using SHWDTech.IOT.CharingPileApi.Models;

namespace SHWDTech.IOT.CharingPileApi.Controllers
{
    public class CommandController : BasicApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> PostAsync(CommandPostViewModel model)
        {
            var result = await BusinessHandler.DispatchCommandAsync(model.TargetIdentity, model.CommandName, model.Pars);
            if (result == null) return Content(HttpStatusCode.InternalServerError, "build command failed");
            if (!result.Successed) return Content(HttpStatusCode.InternalServerError, string.Join(";", result.Errors));
            return Ok();
        }
    }
}
