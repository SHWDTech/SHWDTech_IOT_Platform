using SHWDTech.IOT.Storage.Authorization;
using WebServerComponent.Filter;

namespace SHWDTech.IOT.CharingPileApi.Filters
{
    public class ChargingPileServiceInvokerProvider : IServiceInvokerProvider
    {
        public IServiceInvoker ResolveServiceInvoker(string id)
        {
            using (var repo = new AuthRepository())
            {
                var invoker = repo.FindServiceInvoker(id);
                if (invoker == null) return null;
                return new ChargingPileServiceInvoker
                {
                    Id = invoker.Id.ToString(),
                    Name = invoker.Name
                };
            }
        }
    }
}