using System.Threading.Tasks;
using ChargingPileBusiness.Models;
using ProtocolCommunicationService.Coding;
using ProtocolCommunicationService.Core;
using SHWDTech.IOT.Storage.Communication.Entities;
using SHWDTech.IOT.Storage.Communication.Repository;
using ChargingPileEncoder;

namespace ChargingPileBusiness
{
    public class ChargingPileBusinessHandler : IBusinessHandler
    {
        private const string BusinessName = nameof(ChargingPile);

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
            if (!(package is ChargingPileProtocolPackage cPackage)) return;
            switch ((ProtocolCommandCategory)cPackage.CmdType)
            {
                case ProtocolCommandCategory.SystemCommand:
                    break;
                case ProtocolCommandCategory.ConfigCommand:
                    break;
                case ProtocolCommandCategory.DataCommuinication:
                    break;
            }
        }

        public async Task<PackageDispatchResult> DispatchCommandAsync(string identityCode,string commandName, string[] pars)
        {
            return await DispatchCommandAsync(FrameEncoder.CreateProtocolPackage(identityCode, commandName, pars));
        }

        public async Task<PackageDispatchResult> DispatchCommandAsync(IProtocolPackage package)
        {
            var result =
                await Task.Factory.StartNew(() => OnPackageDispatcher?.Invoke(new BusinessDispatchPackageEventArgs(package, Business)));
            return result;
        }

        public async Task<ChargingPileStatusResult> GetChargingPileStatusAsync(string identityCode)
        {
            var result = await Task.Factory.StartNew(() => ClientSourceStatus.GetRunningStatus(identityCode));
            return result;
        }

        public async Task<ChargingPileStatusResult[]> GetChargingPileStatus(string[] identityCodes)
        {
            var resultes = new ChargingPileStatusResult[identityCodes.Length];
            for (var i = 0; i < identityCodes.Length; i++)
            {
                resultes[i] = await GetChargingPileStatusAsync(identityCodes[i]);
            }

            return resultes;
        }
    }
}
