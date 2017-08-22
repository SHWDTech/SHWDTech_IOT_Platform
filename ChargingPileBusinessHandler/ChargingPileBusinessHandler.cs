using System;
using System.Threading.Tasks;
using ProtocolCommunicationService.Coding;
using ProtocolCommunicationService.Core;

namespace ChargingPileBusiness
{
    public class ChargingPileBusinessHandler : IBusinessHandler
    {
        public event PackageDispatchHandler OnPackageDispatcher;

        public void OnPackageReceive(IProtocolPackage package)
        {
            throw new NotImplementedException();
        }

        public async Task<PackageDispatchResult> BeginDispatchCommandAsync(IProtocolPackage package)
        {
            var result =
                await Task.Factory.StartNew(
                    () => OnPackageDispatcher?.Invoke(new BusinessDispatchPackageEventArgs(package)));
            return result;
        }
    }
}
