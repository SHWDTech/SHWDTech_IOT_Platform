using System;
using System.Configuration;
using System.Web.Http;
using SHWDTech.IOT.CharingPileApi.Providers;
using SHWDTech.IOT.Storage.Authorization.Repository;
using WebServerComponent.MessageHandler;

namespace SHWDTech.IOT.CharingPileApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var authenticationName = ConfigurationManager.AppSettings["AuthName"];
            if(authenticationName == null) throw new ArgumentException("lost application setting AuthName");
            using (var repo = new AuthRepository())
            {
                var schema = repo.FindServiceSchema(authenticationName);
                config.MessageHandlers.Add(new HmacAutheResponseDelegateHandler((ulong)schema.RequestMaxAgeInSeconds, 
                    schema.ServiceSchemaName, 
                    new ChargingPileAllowedAppProvider(schema.ServiceSchemaName)));
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
