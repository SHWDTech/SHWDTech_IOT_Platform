using ProtocolCommunicationService.Coding;
using SHWDTech.IOT.Storage.Communication.Entities;
// ReSharper disable InconsistentNaming

namespace SHWD.ChargingPileBusiness.Models
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

        public string Qrcode { get; set; }

        public RunningStatus Status { get; set; }
    }

    public class ChargingPileClientSource : IClientSource
    {
        public string ClientIdentity { get; set; }

        private string _clientNodeIdString;

        public string ClientNodeIdString
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_clientNodeIdString)) return _clientNodeIdString;
                long number = 0;
                for (var i = 0; i < ClientNodeId.Length; i++)
                {
                    number |= (long)ClientNodeId[i] << ((ClientNodeId.Length - i - 1) * 8);
                }
                _clientNodeIdString = $"{number}";
                return _clientNodeIdString;
            }
        }

        public byte[] ClientNodeId { get; set; }

        public Business Business { get; set; }
    }

    public class ChargingPileApiResult
    {
        public int res { get; set; }

        public string msg { get; set; }

        public string code { get; set; }

        public string nodeid { get; set; }

        public string identitycode { get; set; }

        public RechargeShotInfoResult[] port { get; set; }
    }

    public class RechargeShotInfoResult
    {
        public int index { get; set; }

        public string identitycode { get; set; }

        public string qrimg { get; set; }
    }
}
