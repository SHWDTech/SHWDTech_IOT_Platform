using System;
using System.Net;
using System.Net.Http;

namespace SHWDTech.IOT.CharingPileApi.Controllers
{
    public class ServerInfoController : BasicApiController
    {
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.Accepted, $"Access Ok,Runtime Version:{Environment.Version}, OS Version:{Environment.OSVersion}");
        }
    }
}
