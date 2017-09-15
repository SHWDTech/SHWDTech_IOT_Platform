using System;
using SHWD.ChargingPileEncoder;
using SHWDTech.IOT.Storage.Communication.Repository;

namespace SHWD.ChargingPileBusiness
{
    public class PackageDispatcher
    {
        private string _mobileServerAddr;

        private readonly MobileServerApi _serverApi;

        private PackageDispatcher()
        {
            _serverApi = new MobileServerApi(_mobileServerAddr);
        }

        static PackageDispatcher()
        {
            Instance = new PackageDispatcher();
            using (var repo = new CommunicationProticolRepository())
            {
                Instance._mobileServerAddr = repo.FindMobileServerAddrByBusinessId(ChargingPileBusinessHandler.HandledBusiness.Id);
            }
        }

        public static PackageDispatcher Instance { get; }

        public static void Dispatch(ChargingPileProtocolPackage package)
        {
            switch ((ProtocolCommandCategory)package.CmdType)
            {
                case ProtocolCommandCategory.SystemCommand:
                    Instance.Response(package);
                    break;
                case ProtocolCommandCategory.ConfigCommand:
                    Instance.Response(package);
                    break;
                case ProtocolCommandCategory.DataCommuinication:
                    Instance.Receive(package);
                    break;
            }
        }

        private void Response(ChargingPileProtocolPackage package)
        {

        }

        private void Receive(ChargingPileProtocolPackage package)
        {
            foreach (var dataObject in package.PackageDataObjects)
            {
                if (dataObject.Target == 0x01)
                {
                    ProcessChargingPileReceive(package, dataObject);
                }
                else
                {
                    ProcessRechargeShotReceive(package, dataObject);
                }
            }
        }

        private void ProcessChargingPileReceive(ChargingPileProtocolPackage package, ChargingPilePackageDataObject dataObject)
        {
            if (dataObject.DataContentType == (int)ChargingPileDataType.SelfTest)
            {
                _serverApi.ControlResultReturn(MobileServerApi.ResultTypeSeftTest,
                    $"{dataObject.DataBytes[0]}", package.NodeIdString);
            }
        }

        private void ProcessRechargeShotReceive(ChargingPileProtocolPackage package, ChargingPilePackageDataObject dataObject)
        {
            var shot = ClientSourceStatus.FindRechargShotByIndex(package.ClientSource.ClientIdentity,
                dataObject.Target - 2);
            switch (dataObject.DataContentType)
            {
                case (int) RechargeShotDataType.StartCharging:
                    _serverApi.ControlResultReturn(MobileServerApi.ResultTypeChargingStart,
                        $"{dataObject.DataBytes[0] == 0}", shot.IdentityCode);
                    return;
                case (int)RechargeShotDataType.StopCharging:
                    _serverApi.ControlResultReturn(MobileServerApi.ResultTypeChargingStop,
                        $"{dataObject.DataBytes[0] == 0}", shot.IdentityCode);
                    return;
                case (int)RechargeShotDataType.ChargingAmount:
                    _serverApi.ControlResultReturn(MobileServerApi.ResultTypeChargDatas,
                        $"{BitConverter.ToUInt32(dataObject.DataBytes, 0)}", shot.IdentityCode);
                    return;
            }
        }
    }
}
