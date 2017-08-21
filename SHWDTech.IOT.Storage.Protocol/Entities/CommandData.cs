using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SHWDTech.IOT.Storage.Convention;

namespace SHWDTech.IOT.Storage.Communication.Entities
{
    [Serializable]
    public class CommandData : DataItem<Guid>
    {
        public int DataIndex { get; set; }

        public int DataLength { get; set; }

        public string DataName { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Required]
        public DataDecodeMethod DataDecodeMethod { get; set; }

        public byte DataIndexFlag { get; set; }

        public int ValidFlagIndex { get; set; }

        public virtual ICollection<ProtocolCommand> Commands { get; set; }
    }
}
