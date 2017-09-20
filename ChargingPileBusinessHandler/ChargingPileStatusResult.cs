using System.Collections.Generic;

namespace SHWD.ChargingPileBusiness
{
    public class ChargingPileStatusResult
    {
        public string Identity { get; set; }

        public RunningStatus Status { get; set; }

        public List<RechargShotStatusResult> RechargeShotStatus { get; set; }
    }

    public class RechargShotStatusResult
    {
        public string Identity { get; set; }

        public RunningStatus Status { get; set; }

    }
}
