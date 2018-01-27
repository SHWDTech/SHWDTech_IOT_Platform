using System;
using System.Collections.Generic;
using System.Reflection;
using BasicUtility;
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
                LogService.Instance.Error($@"{DateTime.Now:yyyy-MM-dd HH:mm:ss} start packageEncode => commandName:{commandName}");
                var encoder = Activator.CreateInstance(Assembly.GetExecutingAssembly().GetName().FullName,
                    $"SHWD.ChargingPileBusiness.ProtocolEncoder.{commandName}Encoder").Unwrap() as IFrameEncoder;
                var package = encoder?.Encode(identityCode, pars);
                AddClientInfo(package, identityCode);
                LogService.Instance.Error($@"{DateTime.Now:yyyy-MM-dd HH:mm:ss} packageEncoded => commandName:{commandName}");
                return package;
            }
            catch (Exception ex)
            {
                LogService.Instance.Error($@"{DateTime.Now:yyyy-MM-dd HH:mm:ss} packageEncod Failed => commandName:{commandName}, Exception:{ex.Message}", ex);
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

        protected virtual void CalcDataLength(ChargingPileProtocolPackage package)
        {
            var datalength = 0;
            var dataComponent = package["Data"];
            if (dataComponent != null)
            {
                datalength = dataComponent.ComponentContent.Length;
            }
            var totalLengthBytes = BitConverter.GetBytes((short)datalength);

            package["ContentLength"] = new PackageComponent
            {
                ComponentContent = totalLengthBytes,
                ComponentIndex = 6,
                ComponentName = "ContentLength"
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
