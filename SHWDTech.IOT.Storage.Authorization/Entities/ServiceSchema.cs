using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SHWDTech.IOT.Storage.Convention;

namespace SHWDTech.IOT.Storage.Authorization.Entities
{
    public class ServiceSchema : DataItem<Guid>
    {
        [MaxLength(256)]
        [Index("Ix_SchemaName", IsUnique = true)]
        public string SchemaName { get; set; }

        [MaxLength(256)]
        public string ServiceSchemaName { get; set; }

        [Required]
        public ulong RequestMaxAgeInSeconds { get; set; }
    }
}
