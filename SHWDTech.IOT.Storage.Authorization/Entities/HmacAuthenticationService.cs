using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SHWDTech.IOT.Storage.Convention;

namespace SHWDTech.IOT.Storage.Authorization.Entities
{
    public class HmacAuthenticationService : IDataItem<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        [Index("Ix_ServiceName", IsUnique = true)]
        public string AuthenticationName { get; set; }

        public string AppId { get; set; }

        public string ServiceApiKey { get; set; }
    }
}
