using System.Collections.Generic;
using SHWDTech.IOT.Storage.Authorization;
using SHWDTech.IOT.Storage.Authorization.Entities;
using WebServerComponent.MessageHandler;

namespace SHWDTech.IOT.CharingPileApi.Providers
{
    public class ChargingPileAllowedAppProvider : IAllowedAppProvider
    {
        private readonly Dictionary<string, HmacAuthenticationService> _hmacAuthenticationServices =
            new Dictionary<string, HmacAuthenticationService>();

        private readonly string _authenticationName;

        public ChargingPileAllowedAppProvider(string authenticationName)
        {
            _authenticationName = authenticationName;
        }

        public bool IsAllowedApp(string appId)
        {
            if (_hmacAuthenticationServices.ContainsKey(appId)) return true;
            using (var repo = new AuthRepository())
            {
                var service = repo.FindHmacAuthenticationServiceByAppId(_authenticationName, appId);
                if (service == null) return false;
                _hmacAuthenticationServices.Add(service.AppId, service);
                return true;
            }
        }

        public string this[string appId] => _hmacAuthenticationServices[appId].ServiceApiKey;
    }
}