using System;
using System.Collections.Generic;
using System.Linq;
using SHWDTech.IOT.Storage.Communication;
using SHWDTech.IOT.Storage.Communication.Entities;

namespace ProtocolCommunicationService.Core
{
    public static class BusinessLoader
    {
        private static readonly Dictionary<Guid, Business> Businesses = new Dictionary<Guid, Business>();

        public static Business LoadBusiness(Guid businessId)
        {
            if (Businesses.ContainsKey(businessId)) return Businesses[businessId];
            using (var ctx = new CommunicationProtocolDbContext(ServiceControl.DbConnString))
            {
                var business = ctx.Businesses.FirstOrDefault(b => b.Id == businessId);
                if (business == null) return null;
                Businesses.Add(businessId, business);
                return business;
            }
        }
    }
}
