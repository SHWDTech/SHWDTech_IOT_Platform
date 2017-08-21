using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BasicUtility;
using SHWDTech.IOT.Storage.ChargingPile.Repository;
using SHWDTech.IOT.Storage.ChargingPile.ViewModels;

namespace SHWDTech.IOT.CharingPileApi.Controllers
{
    public class ChargingpileController : ApiController
    {
        private readonly ChargingPileRepository _repo;

        public ChargingpileController()
        {
            _repo = new ChargingPileRepository();
        }

        public async Task<HttpResponseMessage> Put([FromBody]ChargingPileViewModel model)
        {
            try
            {
                var result = await _repo.RegisterChargingPile(model);
                if (!result.Succeeded)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Conflict, string.Join(".", result.Errors));
                }
            }
            catch (Exception ex)
            {
                LogService.Instance.Error("Create ChargingPile Failed", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Create ChargingPile Failed");
            }
            return Request.CreateErrorResponse(HttpStatusCode.Created, "Create Succeeded");
        }
    }
}
