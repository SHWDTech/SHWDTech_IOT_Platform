using System;
using SHWD.ChargingPileEncoder;
using SHWDTech.IOT.Storage.Communication.Repository;

namespace SHWD.ChargingPileBusiness
{
    public class PackageDispatcher
    {
        public static string MobileServerAddr { get; }

        private MobileServerApi _requestServerAip;

        public PackageDispatcher()
        {
            _requestServerAip = new MobileServerApi(MobileServerAddr);
        }

        static PackageDispatcher()
        {
            using (var repo = new CommunicationProtocolRepository())
            {
                MobileServerAddr = repo.FindMobileServerAddrByBusinessId(ChargingPileBusinessHandler.HandledBusiness.Id);
            }

            Instance = new PackageDispatcher();
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
                var ret =_requestServerAip.ControlResultReturn(MobileServerApi.ResultTypeSeftTest,
                    $"{dataObject.DataBytes[0]}", package.NodeIdString);
                Console.WriteLine(ret.Result);
            }
        }

        private void ProcessRechargeShotReceive(ChargingPileProtocolPackage package, ChargingPilePackageDataObject dataObject)
        {
            var shot = ClientSourceStatus.FindRechargShotByIndex(package.ClientSource.ClientIdentity,
                dataObject.Target - 2);
            switch (dataObject.DataContentType)
            {
                case (int)RechargeShotDataType.StartCharging:
                    _requestServerAip.ControlResultReturn(MobileServerApi.ResultTypeChargingStart,
                        $"{dataObject.DataBytes[0] == 0}", shot.IdentityCode);
                    return;
                case (int)RechargeShotDataType.StopCharging:
                    _requestServerAip.ControlResultReturn(MobileServerApi.ResultTypeChargingStop,
                        $"{dataObject.DataBytes[0] == 0}", shot.IdentityCode);
                    return;
                case (int)RechargeShotDataType.ChargingAmount:
                    _requestServerAip.ControlResultReturn(MobileServerApi.ResultTypeChargDatas,
                        $"{BitConverter.ToUInt32(dataObject.DataBytes, 0)}", shot.IdentityCode);
                    return;
            }
        }
    }
}
