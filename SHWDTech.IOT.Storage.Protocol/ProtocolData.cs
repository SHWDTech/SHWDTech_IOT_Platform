using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SHWDTech.IOT.Storage.Convention;

namespace SHWDTech.IOT.Storage.Communication
{
    public class ProtocolData : DataItem<long>
    {
        [Required]
        public Guid BusinessId { get; set; }

        [ForeignKey(nameof(BusinessId))]
        public virtual Business Business { get; set; }

        [Required]
        public long DeviceId { get; set; }

        [ForeignKey(nameof(DeviceId))]
        [Index("Ix_Device_UpdateTime", IsClustered = true, Order = 0)]
        public Device Device { get; set; }

        [Required]
        public byte[] ProtocolContent { get; set; }

        [Required]
        public int ContentLength { get; set; }

        [Required]
        public Guid ProtocolId { get; set; }

        [ForeignKey(nameof(ProtocolId))]
        public Protocol Protocol { get; set; }

        [Required]
        public DateTime DecodeDateTime { get; set; }

        [Required]
        [Index("Ix_Device_UpdateTime", IsClustered = true, Order = 1)]
        public DateTime UpdateDateTime { get; set; }
    }
}
