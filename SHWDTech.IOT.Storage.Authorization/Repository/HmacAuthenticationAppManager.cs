using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using SHWDTech.IOT.Storage.Authorization.Entities;

namespace SHWDTech.IOT.Storage.Authorization.Repository
{
    public class HmacAuthenticationAppManager
    {
        private const string AlphanumericValues = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static async Task<bool> CreateAsync(string authenticationName, string appName)
        {
            var appId = Guid.NewGuid().ToString("N");
            var hmacApplication = new HmacAuthenticationService
            {
                AuthenticationName = authenticationName,
                AppId = appId,
                AppName = appName,
                ServiceApiKey = GenerateAppKey()
            };

            using (var ctx = new AuthorizationDbContext())
            {
                ctx.HmacAuthenticationServices.Add(hmacApplication);
                var ret = await ctx.SaveChangesAsync();
                return ret > 0;
            }
        }

        private static string GenerateAppKey()
        {
            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                var rd = new Random();
                var secretKeyByteArray = Enumerable.Repeat(AlphanumericValues, 32).Select(s => (byte)s[rd.Next(s.Length)]).ToArray();
                cryptoProvider.GetBytes(secretKeyByteArray);
                return Convert.ToBase64String(secretKeyByteArray);
            }
        }
    }
}
