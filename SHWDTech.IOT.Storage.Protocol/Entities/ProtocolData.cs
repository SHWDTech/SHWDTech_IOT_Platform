using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SHWDTech.IOT.Storage.Convention;

namespace SHWDTech.IOT.Storage.Communication.Entities
{
    public class ProtocolData : DataItem<long>
    {
        [Required]
        [Index("Ix_Business_Type_Device_UpdateTime", IsClustered = true, Order = 0)]
        public Guid BusinessId { get; set; }

        [Required]
        [Index("Ix_Business_Type_Device_UpdateTime", IsClustered = true, Order = 1)]
        public ProtocolDataType Type { get; set; }

        [Required]
        [Index("Ix_Business_Type_Device_UpdateTime", IsClustered = true, Order = 2)]
        public long DeviceId { get; set; }

        [Required]
        public byte[] ProtocolContent { get; set; }

        [Required]
        public int ContentLength { get; set; }

        public Guid? ProtocolId { get; set; }

        public DateTime? DecodeDateTime { get; set; }

        [Required]
        public DateTime UpdateDateTime { get; set; }
    }
}
