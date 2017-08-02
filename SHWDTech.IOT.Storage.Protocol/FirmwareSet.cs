using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SHWDTech.IOT.Storage.Convention;

namespace SHWDTech.IOT.Storage.Communication
{
    [Serializable]
    public class FirmwareSet : DataItem<Guid>
    {
        [Required]
        [Display(Name = "固件集名称")]
        [MaxLength(200)]
        public string FirmwareSetName { get; set; }

        public virtual ICollection<Firmware> Firmwares { get; set; }
    }
}
