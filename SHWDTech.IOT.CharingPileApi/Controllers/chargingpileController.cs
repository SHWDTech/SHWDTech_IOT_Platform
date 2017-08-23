using System;
using System.Net;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http;
using BasicUtility;
using SHWDTech.IOT.Storage.ChargingPile.Repository;
using SHWDTech.IOT.Storage.ChargingPile.ViewModels;
using SHWDTech.IOT.Storage.Communication.Repository;

namespace SHWDTech.IOT.CharingPileApi.Controllers
{
    public class ChargingPileController : BasicApiController
    {
        private readonly ChargingPileRepository _repo;

        public ChargingPileController()
        {
            _repo = new ChargingPileRepository();
        }

        public async Task<IHttpActionResult> PutAsync([FromBody]ChargingPileViewModel model)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    var result = await _repo.RegisterChargingPileAsync(model);
                    var commRepo = new CommunicationProticolRepository();
                    var ret = commRepo.RegisterDevice("ChargingPile", model.IdentityCode,
                        BitConverter.GetBytes(long.Parse(model.NodeId)));
                    if (!result.Succeeded || !ret)
                    {
                        return Content(HttpStatusCode.Conflict, string.Join(".", result.Errors));
                    }
                    commRepo.Dispose();
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                LogService.Instance.Error("Create ChargingPile Failed", ex);
                return Content(HttpStatusCode.InternalServerError, "Create ChargingPile Failed");
            }
            return StatusCode(HttpStatusCode.Accepted);
        }

        [HttpGet]
        [Route("api/ChargingPile/{identityCode:string}/Status")]
        public async Task<IHttpActionResult> GetStatusAsync(string identityCode)
        {
            var status = await BusinessHandler.GetChargingPileStatus(identityCode);
            return Ok(status);
        }
    }
}
