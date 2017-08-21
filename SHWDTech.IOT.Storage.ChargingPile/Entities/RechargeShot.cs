using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SHWDTech.IOT.Storage.Convention;

namespace SHWDTech.IOT.Storage.ChargingPile.Entities
{
    public class RechargeShot : DataItem<long>
    {
        [Index("Ix_IdentityCode", IsUnique = true)]
        [MaxLength(256)]
        public string IdentityCode { get; set; }

        [Index("Ix_ChargingPileId_PortNumber", IsUnique = true, Order = 0)]
        public long ChargingPileId { get; set; }

        [ForeignKey(nameof(ChargingPileId))]
        public virtual ChargingPile ChargingPile { get; set;}

        [Index("Ix_ChargingPileId_PortNumber", IsUnique = true, Order = 0)]
        public ushort PortNumber { get; set; }
    }
}
