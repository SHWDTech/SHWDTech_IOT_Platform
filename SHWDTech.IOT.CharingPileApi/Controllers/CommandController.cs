using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SHWDTech.IOT.CharingPileApi.Models;

namespace SHWDTech.IOT.CharingPileApi.Controllers
{
    public class CommandController : BasicApiController
    {
        public async Task<HttpResponseMessage> Post(CommandPostViewModel model)
        {
            var result = await BusinessHandler.BeginDispatchCommandAsync(null);
            if (!result.Successed)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, string.Join(";", result.Errors));
            }

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }
    }
}
