using System;
using System.Web.Http;

namespace SHWDTech.IOT.CharingPileApi.Controllers
{
    public class ServerInfoController : BasicApiController
    {
        public IHttpActionResult Get()
        {
            return Ok($"Access Ok,Runtime Version:{Environment.Version}, OS Version:{Environment.OSVersion}");
        }
    }
}
