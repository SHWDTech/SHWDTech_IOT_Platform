using System.Collections.Generic;
using System.Linq;
using SHWD.ChargingPileBusiness.Models;

namespace SHWD.ChargingPileBusiness
{
    public class ClientSourceStatus
    {
        private static readonly Dictionary<string, ChargingPile> ClientStatus;

        private ClientSourceStatus()
        {

        }

        static ClientSourceStatus()
        {
            ClientStatus = new Dictionary<string, ChargingPile>();
        }

        public static void UpdateRunningStatus(string identityCode, RunningStatus status)
        {
            ChargingPile client;
            if (!ClientStatus.ContainsKey(identityCode))
            {
                client = new ChargingPile();
                ClientStatus.Add(identityCode, client);
            }
            else
            {
                client = ClientStatus[identityCode];
            }

            client.Status = status;
        }

        public static void UpdateRechargeShotRunningStatus(string pileIdentity, string identityCode, RunningStatus status)
        {
            ChargingPile client;
            if (!ClientStatus.ContainsKey(pileIdentity))
            {
                client = new ChargingPile();
                ClientStatus.Add(pileIdentity, client);
            }
            else
            {
                client = ClientStatus[pileIdentity];
            }

            var shot = client.RechargShots.FirstOrDefault(s => s.IdentityCode == identityCode);
            if (shot == null) return;
            shot.Status = status;
        }

        public static ChargingPileStatusResult GetRunningStatus(string identityCode)
        {
            if (!ClientStatus.ContainsKey(identityCode)) return null;
            var client = ClientStatus[identityCode];
            var result = new ChargingPileStatusResult
            {
                Identity = client.IdentityCode,
                Status = client.Status
            };
            if (client.RechargShots.Length <= 0) return result;
            result.RechargeShotStatus = new List<ChargingPileStatusResult>();
            foreach (var shot in client.RechargShots)
            {
                result.RechargeShotStatus.Add(new ChargingPileStatusResult
                {
                    Identity = shot.IdentityCode,
                    Status = shot.Status
                });
            }
            return result;
        }

        public static RechargShot FindRechargShotByIndex(string pileIdentity, int index)
        {
            ChargingPile client;
            if (!ClientStatus.ContainsKey(pileIdentity))
            {
                client = new ChargingPile();
                ClientStatus.Add(pileIdentity, client);
            }
            else
            {
                client = ClientStatus[pileIdentity];
            }

            if (index > client.RechargShots.Length - 1) return null;
            return client.RechargShots[index];
        }
    }
}
