namespace ChargingPileBusiness.Models
{
    public class ChargingPile
    {
        public string IdentityCode { get; set; }

        public string NodeId { get; set; }

        public RunningStatus Status { get; set; }

        public RechargShot[] RechargShots { get; set; }
    }

    public class RechargShot
    {
        public string IdentityCode { get; set; }

        public RunningStatus Status { get; set; }
    }
}
