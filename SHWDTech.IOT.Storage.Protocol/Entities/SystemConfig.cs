using System;
using System.ComponentModel.DataAnnotations;
using SHWDTech.IOT.Storage.Convention;

namespace SHWDTech.IOT.Storage.Communication.Entities
{
    [Serializable]
    public class SystemConfig : DataItem<Guid>
    {
        public Guid? BusinessId { get; set; }

        [Required]
        [MaxLength(256)]
        public string ItemType { get; set; }

        [Required]
        [MaxLength(512)]
        public string ItemKey { get; set; }

        [Required]
        [MaxLength(2048)]
        public string ItemValue { get; set; }
    }
}
