using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SHWDTech.IOT.Storage.Convention;

namespace SHWDTech.IOT.Storage.Communication
{
    [Serializable]
    public class Firmware : DataItem<Guid>
    {
        [Required]
        [MaxLength(200)]
        public string FirmwareName { get; set; }

        public virtual ICollection<FirmwareSet> FirmwareSets { get; set; }

        public virtual ICollection<Protocol> Protocols { get; set; }
    }
}
