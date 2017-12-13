using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProtocolCommunicationService.Coding;
using ProtocolCommunicationService.Core;
using SHWD.ChargingPileBusiness.Models;
using SHWD.ChargingPileBusiness.ProtocolEncoder;
using SHWD.ChargingPileEncoder;
using SHWDTech.IOT.Storage.Communication.Entities;
using SHWDTech.IOT.Storage.Communication.Repository;

namespace SHWD.ChargingPileBusiness
{
    public class ChargingPileBusinessHandler : IBusinessHandler
    {
        private const string BusinessName = nameof(ChargingPile);

        public Business Business { get; }

        public event PackageDispatchHandler OnPackageDispatcher;

        public static Business HandledBusiness { get; private set; }

        public ChargingPileBusinessHandler()
        {
            using (var repo = new CommunicationProtocolRepository())
            {
                Business = repo.FindBusinessByNameAsync(BusinessName).Result;
                HandledBusiness = Business;
            }
        }

        public ReceiveFeedback[] OnPackageReceive(IProtocolPackage package)
        {
            if (!(package is ChargingPileProtocolPackage cPackage)) return null;
            return PackageDispatcher.Dispatch(cPackage);
        }

        public IClientSource FindClientSource(IProtocolPackage package)
        {
            IClientSource clientSource;
            try
            {
                var ret = new MobileServerApi(PackageDispatcher.MobileServerAddr).GetChargingPileIdentityInformation(package.NodeIdString);
                var result = JsonConvert.DeserializeObject<ChargingPileApiResult>(ret.Result);
                UpdateStatus(result);
                clientSource = new ChargingPileClientSource
                {
                    Business = Business,
                    ClientIdentity = result.identitycode,
                    ClientNodeId = package.DeviceNodeId
                };
            }
            catch (Exception)
            {
                clientSource = null;
            }
            return clientSource;
        }

        public void ClientAuthenticated(ClientAuthenticatedArgs args)
        {
            Task.Factory.StartNew(() => DispatchCommandAsync(args.AuthenticatedClientSource.ClientIdentity, "SystemTime", null));
        }

        private void UpdateStatus(ChargingPileApiResult result)
        {
            ClientSourceStatus.UpdateRunningStatus(result.identitycode, result.nodeid, RunningStatus.OnLine);
            ClientSourceStatus.UpdateRechargeShotRunningStatus(result.identitycode, result.port, RunningStatus.OnLine);
        }

        public async Task<PackageDispatchResult> DispatchCommandAsync(string identityCode, string commandName, Dictionary<string, string> pars)
        {
            return await DispatchCommandAsync(FrameEncoderBase.CreateProtocolPackage(identityCode, commandName, pars));
        }

        public async Task<PackageDispatchResult> DispatchCommandAsync(IProtocolPackage package)
        {
            var result =
                await Task.Factory.StartNew(() => OnPackageDispatcher?.Invoke(new BusinessDispatchPackageEventArgs(package, Business)));
            if (result.Successed)
            {
                result.RequestCode = package.RequestCode;
            }
            return result;
        }

        public async Task<ChargingPileStatusResult> GetChargingPileStatusAsync(string identityCode)
        {
            var result = await Task.Factory.StartNew(() => ClientSourceStatus.GetRunningStatus(identityCode));
            return result;
        }

        public async Task<ChargingPileStatusResult[]> GetChargingPileStatusAsync(string[] identityCodes)
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
