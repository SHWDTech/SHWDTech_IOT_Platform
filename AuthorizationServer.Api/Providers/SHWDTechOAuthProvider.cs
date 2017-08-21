using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using SHWDTech.IOT.Storage.Authorization;
using SHWDTech.IOT.Storage.Authorization.Entities;

// ReSharper disable InconsistentNaming

namespace AuthorizationServer.Api.Providers
{
    public class SHWDTechOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly AuthRepository _repo;

        public SHWDTechOAuthProvider()
        {
            _repo = new AuthRepository();
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (!context.TryGetBasicCredentials(out string clientId, out string clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                context.SetError("invalid_clientId", "client_Id is not set");
                return Task.FromResult<object>(null);
            }
            var audience = _repo.FindAudience(clientId);

            if (audience == null)
            {
                context.SetError("invalid_clientId", $"Invalid client_id '{context.ClientId}'");
            }

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            if (context.UserName != context.Password)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect");
                return Task.FromResult<object>(null);
            }

            var identity = new ClaimsIdentity("JWT");

            HandleClaims(context, identity);

            var props = new AuthenticationProperties(new Dictionary<string, string>
            {
                {
                    "audience", context.ClientId ?? string.Empty
                }
            });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
            return Task.FromResult<object>(null);
        }

        private void HandleClaims(OAuthGrantResourceOwnerCredentialsContext context, ClaimsIdentity identity)
        {
            var audience = _repo.FindAudience(context.ClientId);
            switch (audience.AudienceType)
            {
                    case AudienceType.ApiService:
                    HandleApiClaims(audience, identity);
                        break;
            }

        }

        private static void HandleApiClaims(Audience audience, ClaimsIdentity identity)
        {
            identity.AddClaim(new Claim(nameof(AudienceType), audience.AudienceType.ToString()));
            identity.AddClaim(new Claim("AudienceName", audience.Name));
        }
    }
}