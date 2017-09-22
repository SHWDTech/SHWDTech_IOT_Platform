using System;
using System.Collections.Generic;
using System.Reflection;
using ProtocolCommunicationService.Coding;
using SHWD.ChargingPileEncoder;

namespace SHWD.ChargingPileBusiness.ProtocolEncoder
{
    public abstract class FrameEncoderBase : IFrameEncoder
    {
        public static ChargingPileProtocolPackage CreateProtocolPackage(string identityCode, string commandName, Dictionary<string, string> pars)
        {
            try
            {
                var encoder = Activator.CreateInstance(Assembly.GetExecutingAssembly().GetName().FullName,
                    $"SHWD.ChargingPileBusiness.ProtocolEncoder.{commandName}Encoder").Unwrap() as IFrameEncoder;
                var package = encoder?.Encode(identityCode, pars);
                AddClientInfo(package, identityCode);
                return package;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public abstract ChargingPileProtocolPackage Encode(string identity, Dictionary<string, string> pars);

        protected virtual void GenerateRequestCode(ChargingPileProtocolPackage package)
        {
            var now = DateTime.Now;
            var code = new byte[8];
            code[0] = (byte) (now.Year - 2000);
            code[1] = (byte) now.Month;
            code[2] = (byte) now.Day;
            code[3] = (byte) now.Hour;
            code[4] = (byte) now.Minute;
            code[5] = (byte) now.Second;
            var millisecondBytes = BitConverter.GetBytes((short) now.Millisecond);
            code[6] = millisecondBytes[0];
            code[7] = millisecondBytes[1];
            var request = new RequestCode(code);
            package.RequestCodeComponent = request;
        }

        protected virtual void AddModbus(ChargingPileProtocolPackage package)
        {
            var crcBytes = BitConverter.GetBytes(ChargingPileProtocolPackage.GetCrcModBus(package.GetBytes()));
            Array.Reverse(crcBytes);
            package["CrcModBus"] = new PackageComponent
            {
                ComponentContent = crcBytes,
                ComponentIndex = 8,
                ComponentName = "CrcModBus"
            };
        }

        private static void AddClientInfo(IProtocolPackage package, string identity)
        {
            if (package == null) return;
            var chargingPile = ClientSourceStatus.FindChargingPileByIdentity(identity);
            if (chargingPile == null) return;
            var nodeidArray = BitConverter.GetBytes(long.Parse(chargingPile.NodeId));
            Array.Reverse(nodeidArray);
            package.DeviceNodeId = nodeidArray;
        }
    }
}
