using System.Data.Entity;
using SHWDTech.IOT.Storage.ChargingPile.Entities;

namespace SHWDTech.IOT.Storage.ChargingPile
{
    public class ChargingPileDbContext : DbContext
    {
        private const string DefaultConnName = "ChargingPileDB";

        public ChargingPileDbContext() : base(DefaultConnName)
        {
            
        }

        public ChargingPileDbContext(string connStr) : base(connStr)
        {
            
        }

        public virtual IDbSet<Entities.ChargingPile> ChargingPiles { get; set; }

        public virtual IDbSet<RechargeShot> RechargeShots { get; set; }
    }
}
