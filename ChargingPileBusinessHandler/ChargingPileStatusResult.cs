using System.Collections.Generic;

namespace SHWD.ChargingPileBusiness
{
    public class ChargingPileStatusResult
    {
        public string Identity { get; set; }

        public RunningStatus Status { get; set; }

        public List<ChargingPileStatusResult> RechargeShotStatus { get; set; }
    }
}
