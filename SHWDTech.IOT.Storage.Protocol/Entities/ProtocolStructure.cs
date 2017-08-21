using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SHWDTech.IOT.Storage.Convention;

namespace SHWDTech.IOT.Storage.Communication.Entities
{
    [Serializable]
    public class ProtocolStructure : DataItem<Guid>
    {
        [Required]
        public Guid ProtocolId { get; set; }

        [ForeignKey(nameof(ProtocolId))]
        public virtual Protocol Protocol { get; set; }

        [Required]
        public DataDecodeMethod DataDecodeMethod { get; set; }

        [Required]
        [MaxLength(50)]
        public string StructureName { get; set; }

        public int StructureIndex { get; set; }

        public int StructureDataLength { get; set; }

        [MaxLength(256)]
        public byte[] DefaultBytes { get; set; }
    }
}
