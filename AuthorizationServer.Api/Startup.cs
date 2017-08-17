using System;
using System.Linq;
using System.Web.Http;
using AuthorizationServer.Api.Formats;
using AuthorizationServer.Api.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using SHWDTech.IOT.Storage.Authorization;

namespace AuthorizationServer.Api
{
    public class Startup
    {
        private readonly AuthorizationDbContext _ctx;

        public Startup()
        {
            _ctx = new AuthorizationDbContext();
        }

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            ConfigureOAuth(app);

            app.UseCors(CorsOptions.AllowAll);

            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            var oAuthConfigs = _ctx.SystemConfigs.Where(item => item.ItemType == "OAuth").ToDictionary(k => k.ItemName, v => v.ItemValue);
            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString(oAuthConfigs[nameof(PathString)]),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(15),
                Provider = new SHWDTechOAuthProvider(),
                AccessTokenFormat = new SHWDTechJwtFormat(oAuthConfigs["OAuthDomain"])
            };

            app.UseOAuthAuthorizationServer(oAuthServerOptions);
        }
    }
}