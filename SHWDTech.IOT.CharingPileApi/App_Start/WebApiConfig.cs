using System.Web.Configuration;
using System.Web.Http;
using SHWDTech.IOT.CharingPileApi.Providers;
using SHWDTech.IOT.Storage.Authorization;
using WebServerComponent.MessageHandler;

namespace SHWDTech.IOT.CharingPileApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var rootWebConfig1 = WebConfigurationManager.OpenWebConfiguration(null);
            var authenticationName =
                rootWebConfig1.AppSettings.Settings["AuthenticationName"];
            using (var repo = new AuthRepository())
            {
                var schema = repo.FindServiceSchema("cpx");
                config.MessageHandlers.Add(new HmacAuthenticationDelegateHandler(schema.RequestMaxAgeInSeconds, 
                    schema.ServiceSchemaName, 
                    new ChargingPileAllowedAppProvider(authenticationName.Value)));
            }

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
