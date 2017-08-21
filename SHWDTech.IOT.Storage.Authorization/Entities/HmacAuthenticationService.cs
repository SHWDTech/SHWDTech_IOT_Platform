using System;
using System.ComponentModel.DataAnnotations.Schema;
using SHWDTech.IOT.Storage.Convention;

namespace SHWDTech.IOT.Storage.Authorization.Entities
{
    public class HmacAuthenticationService : DataItem<Guid>
    {
        [Index("Ix_AuthenticationName_AppName_AppId", IsUnique = true, Order = 0)]
        public string AuthenticationName { get; set; }

        [Index("Ix_AuthenticationName_AppName_AppId", IsUnique = true, Order = 1)]
        public string AppName { get; set; }

        [Index("Ix_AuthenticationName_AppName_AppId", IsUnique = true, Order = 2)]
        public string AppId { get; set; }

        public string ServiceApiKey { get; set; }
    }
}
