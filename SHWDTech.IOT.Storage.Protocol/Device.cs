using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using SHWDTech.IOT.Storage.Convention;

namespace SHWDTech.IOT.Storage.Communication
{
    public class Device : DataItem<long>
    {
        private string _nodeIdHexString;

        private long? _nodeIdNumber;

        [Required]
        [Index("Ix_Business_DeviceNodeId_Unique", IsUnique = true, Order = 0)]
        public Guid BusinessId { get; set; }

        [ForeignKey(nameof(BusinessId))]
        public virtual Business Business { get; set; }

        [Required]
        [MaxLength(100)]
        public string DeviceName { get; set; }

        [Required]
        [MaxLength(16)]
        [Index("Ix_Business_DeviceNodeId_Unique", IsUnique = true, Order = 1)]
        public byte[] NodeId { get; set; }

        [MaxLength(512)]
        public byte[] CreditCode { get; set; }

        [Required]
        public EncryptType EncryptType { get; set; }

        [NotMapped]
        public string NodeIdHexString
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_nodeIdHexString)) return _nodeIdHexString;
                var builder = new StringBuilder();
                foreach (var b in NodeId)
                {
                    builder.Append($"{b:X}");
                }
                _nodeIdHexString = builder.ToString();
                return _nodeIdHexString;
            }
        }

        [NotMapped]
        public long NodeIdNumber
        {
            get
            {
                if (_nodeIdNumber != null) return _nodeIdNumber.Value;
                long number = 0;
                for (var i = 0; i < NodeId.Length; i++)
                {
                    number |= (long)NodeId[i] << ((NodeId.Length - i - 1) * 8);
                }
                _nodeIdNumber = number;
                return _nodeIdNumber.Value;
            }
        }

        [NotMapped]
        public string NodeIdString => $"{NodeIdNumber}";
    }
}
