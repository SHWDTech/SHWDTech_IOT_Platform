using System;
using System.ComponentModel.DataAnnotations;
using SHWDTech.IOT.Storage.Convention;

namespace SHWDTech.IOT.Storage.Authorization.Entities
{
    public class SystemConfig : DataItem<Guid>
    {
        [MaxLength(256)]
        public string ItemType { get; set; }

        [MaxLength(256)]
        public string ItemName { get; set; }

        [MaxLength(1024)]
        public string ItemValue { get; set; }
    }
}
