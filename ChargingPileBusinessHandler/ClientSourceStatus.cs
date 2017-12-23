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

        public static void UpdateRunningStatus(string identityCode, string nodeId, RunningStatus status)
        {
            ChargingPile client;
            if (!ClientStatus.ContainsKey(identityCode))
            {
                client = new ChargingPile
                {
                    IdentityCode = identityCode,
                    NodeId = nodeId
                };
                ClientStatus.Add(identityCode, client);
            }
            else
            {
                client = ClientStatus[identityCode];
            }

            client.Status = status;
        }

        public static void UpdateRechargeShotRunningStatus(string pileIdentity, RechargeShotInfoResult[] rechargShot, RunningStatus status)
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
            if (client.RechargShots == null || client.RechargShots.Length != rechargShot.Length)
            {
                client.RechargShots = new RechargShot[rechargShot.Length];
            }
            for (var i = 0; i < rechargShot.Length; i++)
            {
                var shot = client.RechargShots[i] = new RechargShot();
                shot.IdentityCode = rechargShot[i].identitycode;
                shot.Status = status;
                shot.Qrcode = rechargShot[i].qrimg;
            }
        }

        public static void UpdateRechargeShotRunningStatus(string pileIdentity, string shotIdentity, RunningStatus status)
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
            var shot = client.RechargShots?.FirstOrDefault(s => s.IdentityCode == shotIdentity);
            if (shot != null)
            {
                shot.Status = status;
            }
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
            result.RechargeShotStatus = new List<RechargShotStatusResult>();
            foreach (var shot in client.RechargShots)
            {
                result.RechargeShotStatus.Add(new RechargShotStatusResult
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

        public static ChargingPile FindChargingPileByIdentity(string idengtity)
        {
            if (!ClientStatus.ContainsKey(idengtity)) return null;
            return ClientStatus[idengtity];
        }

        public static int GetShotIndexByIdentity(string chargingPileIdentity, string rechargeShotIdentity)
        {
            if (!ClientStatus.ContainsKey(chargingPileIdentity)) return -1;
            var pile = ClientStatus[chargingPileIdentity];
            var index = 1;
            for (var i = 0; i < pile.RechargShots.Length; i++)
            {
                if (pile.RechargShots[i].IdentityCode != rechargeShotIdentity) continue;
                index = i + 1;
                break;
            }
            return index;
        }

        public static ChargingPile GetChargingPileIdentityByNodeId(string nodeIdString)
        {
            var client = ClientStatus.FirstOrDefault(c => c.Value.NodeId == nodeIdString);
            return client.Value;
        }
    }
}
