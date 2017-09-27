using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SHWDTech.IOT.Storage.Convention;

namespace SHWDTech.IOT.Storage.Communication.Entities
{
    public class ProtocolData : DataItem<long>
    {
        [Required]
        public Guid BusinessId { get; set; }

        [Required]
        [Index("Ix_Type_Device_UpdateTime", IsClustered = true, Order = 0)]
        public ProtocolDataType Type { get; set; }

        [Required]
        [Index("Ix_Type_Device_UpdateTime", IsClustered = true, Order = 1)]
        public long DeviceId { get; set; }

        [Required]
        public byte[] ProtocolContent { get; set; }

        [Required]
        public int ContentLength { get; set; }

        public Guid? ProtocolId { get; set; }

        public DateTime? DecodeDateTime { get; set; }

        [Required]
        [Index("Ix_Type_Device_UpdateTime", IsClustered = true, Order = 2)]
        public DateTime UpdateDateTime { get; set; }
    }
}
