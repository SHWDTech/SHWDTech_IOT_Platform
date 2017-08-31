using System;
using System.Linq;
using System.Threading.Tasks;
using SHWDTech.IOT.Storage.Communication.Entities;

namespace SHWDTech.IOT.Storage.Communication.Repository
{
    public class CommunicationProticolRepository : IDisposable
    {
        private readonly CommunicationProtocolDbContext _ctx;

        public CommunicationProticolRepository()
        {
            _ctx = new CommunicationProtocolDbContext();
        }

        public bool RegisterDevice(string businessName, string deviceName, byte[] nodeIdBytes)
        {
            var business = _ctx.Businesses.FirstOrDefault(b => b.BusinessName == businessName);
            if (business == null) return false;
            var dev = new Device
            {
                Business = business,
                DeviceName = deviceName,
                NodeId = nodeIdBytes
            };

            _ctx.Devices.Add(dev);
            return _ctx.SaveChanges() > 0;
        }

        public async Task<Business> FindBusinessByNameAsync(string name)
        {
            Business business = null;
            await Task.Factory.StartNew(() =>
            {
                business = _ctx.Businesses.FirstOrDefault(b => b.BusinessName == name);
            });

            return business;
        }

        public Device FindDeviceByNodeId(Guid businessId, byte[] nodeId)
        {
            return _ctx.Devices.FirstOrDefault(d => d.BusinessId == businessId && d.NodeId == nodeId);
        }

        public string FindMobileServerAddrByBusinessId(Guid businessId)
        {
            return _ctx.SystemConfigs.FirstOrDefault(c =>
                    c.BusinessId == businessId && c.ItemType == "ServerInformation" && c.ItemKey == "MobileServer")
                ?.ItemValue;
        }

        public void Dispose()
        {
            _ctx.Dispose();
        }
    }
}
