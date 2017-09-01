using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SHWDTech.IOT.Storage.Communication.Entities;

namespace SHWDTech.IOT.Storage.Communication.Repository
{
    public class CommunicationProticolRepository : IDisposable
    {
        private readonly CommunicationProtocolDbContext _ctx;

        public static string ConnStr { get; set; }

        public CommunicationProticolRepository()
        {
            _ctx = string.IsNullOrWhiteSpace(ConnStr) 
                ? new CommunicationProtocolDbContext() 
                : new CommunicationProtocolDbContext(ConnStr);
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

        public void SaveProtocolData(ProtocolData protocolData)
        {
            _ctx.ProtocolDatas.Add(protocolData);
            _ctx.SaveChanges();
        }

        public List<Protocol> LoadProtocols(string[] includes = null, Expression<Func<Protocol, bool>> exp = null)
        {
            var source = _ctx.Protocols.AsQueryable();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    source.Include(include);
                }
            }
            if (exp != null)
            {
                source = source.Where(exp);
            }
            return source.ToList();
        }

        public void Dispose()
        {
            _ctx.Dispose();
        }
    }
}
