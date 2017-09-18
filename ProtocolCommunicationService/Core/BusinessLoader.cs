using System;
using System.Collections.Generic;
using SHWDTech.IOT.Storage.Communication.Entities;
using SHWDTech.IOT.Storage.Communication.Repository;

namespace ProtocolCommunicationService.Core
{
    public static class BusinessLoader
    {
        private static readonly Dictionary<Guid, Business> Businesses = new Dictionary<Guid, Business>();

        public static Business LoadBusiness(Guid businessId)
        {
            if (Businesses.ContainsKey(businessId)) return Businesses[businessId];
            using (var repo = new CommunicationProtocolRepository())
            {
                var business = repo.FindBusinessById(businessId);
                if (business == null) return null;
                Businesses.Add(businessId, business);
                return business;
            }
        }
    }
}
