using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SHWDTech.IOT.Storage.Convention;

namespace SHWDTech.IOT.Storage.Communication.Entities
{
    [Serializable]
    public class Protocol : DataItem<Guid>
    {
        [Required]
        [MaxLength(200)]
        public string ProtocolName { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProtocolModule { get; set; }

        [Required]
        [MaxLength(100)]
        public string Version { get; set; }

        [Required]
        [MaxLength(10)]
        public byte[] Head { get; set; }

        [Required]
        [MaxLength(10)]
        public byte[] Tail { get; set; }

        public virtual ICollection<ProtocolStructure> ProtocolStructures { get; set; } = new List<ProtocolStructure>();

        public virtual ICollection<ProtocolCommand> ProtocolCommands { get; set; } = new List<ProtocolCommand>();

        public virtual ICollection<Firmware> Firmwares { get; set; }

        [Required]
        public DateTime ReleaseDateTime { get; set; }

        [Required]
        public ProtocolCheckType CheckType { get; set; }
    }
}
