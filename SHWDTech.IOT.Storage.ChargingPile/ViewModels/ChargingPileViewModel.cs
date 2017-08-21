using System.Collections.Generic;

namespace SHWDTech.IOT.Storage.ChargingPile.ViewModels
{
    public class ChargingPileViewModel
    {
        public string IdentityCode { get; set; }

        public string NodeId { get; set; }

        public List<RechargeShotViewModel> RechargeShots { get; set; } = new List<RechargeShotViewModel>();
    }

    public class RechargeShotViewModel
    {
        public string IdentityCode { get; set; }

        public ushort PortNumber { get; set; }
    }
}