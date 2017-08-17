using System;
using System.ComponentModel.DataAnnotations;
using SHWDTech.IOT.Storage.Convention;

namespace SHWDTech.IOT.Storage.Authorization
{
    public class SystemConfig : IDataItem<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(256)]
        public string ItemType { get; set; }

        [MaxLength(256)]
        public string ItemName { get; set; }

        [MaxLength(1024)]
        public string ItemValue { get; set; }
    }
}
