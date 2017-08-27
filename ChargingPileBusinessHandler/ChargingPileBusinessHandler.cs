using System;
using System.Threading.Tasks;
using ProtocolCommunicationService.Coding;
using ProtocolCommunicationService.Core;
using SHWDTech.IOT.Storage.Communication.Entities;
using SHWDTech.IOT.Storage.Communication.Repository;

namespace ChargingPileBusiness
{
    public class ChargingPileBusinessHandler : IBusinessHandler
    {
        private const string BusinessName = "ChargingPile";

        public Business Business { get; }

        public event PackageDispatchHandler OnPackageDispatcher;

        public ChargingPileBusinessHandler()
        {
            using (var repo = new CommunicationProticolRepository())
            {
                Business = repo.FindBusinessByNameAsync(BusinessName).Result;
            }
        }

        public void OnPackageReceive(IProtocolPackage package)
        {
            throw new NotImplementedException();
        }

        public async Task<PackageDispatchResult> DispatchCommandAsync(string identityCode,string commandName, string[] pars)
        {
            return await DispatchCommandAsync(FrameEncoder.CreateProtocolPackage(identityCode, commandName, pars));
        }

        public async Task<PackageDispatchResult> DispatchCommandAsync(IProtocolPackage package)
        {
            var result =
                await Task.Factory.StartNew(
                    () => OnPackageDispatcher?.Invoke(new BusinessDispatchPackageEventArgs(package, Business)));
            return result;
        }

        public async Task<ChargingPileStatusResult> GetChargingPileStatus(string identityCode)
        {
            return null;
        }

        public async Task<ChargingPileStatusResult[]> GetChargingPileStatus(string[] identityCodes)
        {
            return null;
        }
    }
}
