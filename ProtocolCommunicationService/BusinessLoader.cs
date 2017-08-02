using System;
using System.Collections.Generic;
using System.Linq;
using SHWDTech.IOT.Storage.Communication;

namespace ProtocolCommunicationService
{
    public static class BusinessLoader
    {
        private static readonly Dictionary<Guid, Business> Businesses = new Dictionary<Guid, Business>();

        public static Business LoadBusiness(Guid businessId)
        {
            Business business;
            if (Businesses.ContainsKey(businessId)) return Businesses[businessId];
            using (var ctx = new CommunicationProtocolDbContext(ServiceControl.DbConnString))
            {
                business = ctx.Businesses.FirstOrDefault(b => b.Id == businessId);
                Businesses.Add(businessId, business);
            }

            return business;
        }
    }
}
