using System;
using System.Collections.Generic;
using SHWDTech.IOT.Storage.Convention;
using System.ComponentModel.DataAnnotations;

namespace SHWDTech.IOT.Storage.ChargingPile.Entities
{
    public class ChargingPile : DataItem<long>
    {
        [MaxLength(256)]
        public string IdentityCode { get; set; }

        [MaxLength(32)]
        public string NodeId { get; set; }

        public DateTime CreateDateTime { get; set; }

        public ICollection<RechargeShot> RechargeShots { get; set; }
    }
}
