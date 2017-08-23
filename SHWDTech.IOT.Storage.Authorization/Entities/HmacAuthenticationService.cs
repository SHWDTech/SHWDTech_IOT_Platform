using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SHWDTech.IOT.Storage.Convention;

namespace SHWDTech.IOT.Storage.Authorization.Entities
{
    public class HmacAuthenticationService : DataItem<Guid>
    {
        [MaxLength(32)]
        [Index("Ix_AuthName_AppName_AppId", IsUnique = true, Order = 0)]
        public string AuthName { get; set; }

        [MaxLength(128)]
        [Index("Ix_AuthName_AppName_AppId", IsUnique = true, Order = 1)]
        public string AppName { get; set; }

        [MaxLength(128)]
        [Index("Ix_AuthName_AppName_AppId", IsUnique = true, Order = 2)]
        public string AppId { get; set; }

        public string ServiceApiKey { get; set; }
    }
}
