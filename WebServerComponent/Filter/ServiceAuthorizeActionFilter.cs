using System.Web.Http.Filters;

namespace WebServerComponent.Filter
{
    public abstract class ServiceAuthorizeActionFilter : AuthorizationFilterAttribute
    {
        public abstract IServiceInvokerProvider ServiceInvokerProvider { get; set; }
    }
}
