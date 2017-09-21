using System.Collections.Generic;
using SHWD.ChargingPileEncoder;

namespace SHWD.ChargingPileBusiness.ProtocolEncoder
{
    public interface IFrameEncoder
    {
        ChargingPileProtocolPackage Encode(string identity, Dictionary<string, string> pars);
    }
}
