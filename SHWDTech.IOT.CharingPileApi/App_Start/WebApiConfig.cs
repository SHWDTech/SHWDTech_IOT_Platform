using System.Web.Http;
using SHWDTech.IOT.CharingPileApi.Filters;

namespace SHWDTech.IOT.CharingPileApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Filters.Add(new ChargingPileInvokerAuthorizaActionFilter
            {
                ServiceInvokerProvider = new ChargingPileServiceInvokerProvider()
            });
        }
    }
}
