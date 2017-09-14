using SHWD.ChargingPileEncoder;
using SHWDTech.IOT.Storage.Communication.Repository;

namespace SHWD.ChargingPileBusiness
{
    public class PackageDispatcher
    {
        private string _mobileServerAddr;

        private PackageDispatcher()
        {

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
            if (dataObject.DataContentType == ChargingPileDataType.SelfTest)
            {
                
            }
        }

        private void ProcessRechargeShotReceive(ChargingPileProtocolPackage package, ChargingPilePackageDataObject dataObject)
        {
            var shot = ClientSourceStatus.FindRechargShotByIndex(package.ClientSource.ClientIdentity,
                dataObject.Target - 2);

        }
    }
}
