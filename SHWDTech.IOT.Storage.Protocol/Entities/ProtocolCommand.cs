using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using SHWDTech.IOT.Storage.Convention;

namespace SHWDTech.IOT.Storage.Communication.Entities
{
    [Serializable]
    public class ProtocolCommand : DataItem<Guid>
    {
        [Required]
        public Guid ProtocolId { get; set; }

        [ForeignKey(nameof(ProtocolId))]
        public Protocol Protocol { get; set; }

        [Required]
        [MaxLength(16)]
        public byte[] CommandCode { get; set; }

        public int SendBytesLength { get; set; }

        public int ReceiveBytesLength { get; set; }

        public int MaxReceiveBytesLength { get; set; }

        [Required]
        public string CommandCategory { get; set; }

        [Required]
        public DataOrderType DataOrderType { get; set; }

        public virtual ICollection<CommandData> CommandDatas { get; set; } = new List<CommandData>();

        [Required]
        public string DeliverParamString { get; set; }

        public virtual List<string> DeliverParams
        {
            get
            {
                if (_deliverParams != null) return _deliverParams;
                _deliverParams = DeliverParamString.Split(',').ToList();
                return _deliverParams;
            }
        }

        private List<string> _deliverParams;
    }
}
