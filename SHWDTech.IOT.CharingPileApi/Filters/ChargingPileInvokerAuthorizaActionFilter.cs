using System;
using System.Security.Claims;
using System.Web.Http.Controllers;
using WebServerComponent.Filter;

namespace SHWDTech.IOT.CharingPileApi.Filters
{
    public sealed class ChargingPileInvokerAuthorizaActionFilter : ServiceAuthorizeActionFilter
    {
        public override IServiceInvokerProvider ServiceInvokerProvider { get; set; }


        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if(ServiceInvokerProvider == null) throw new NullReferenceException("ServiceInvokerProvider can not be null");
            var provider =
                ServiceInvokerProvider.ResolveServiceInvoker(actionContext.Request.Properties["SecurityStamp"]
                    .ToString());
            if (provider == null) return;
            actionContext.RequestContext.Principal = new ClaimsPrincipal(new ClaimsIdentity("ServiceInvoker"));
        }

    }
}