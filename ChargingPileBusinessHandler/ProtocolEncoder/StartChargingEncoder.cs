using System;
using System.Collections.Generic;
using ProtocolCommunicationService.Coding;
using SHWD.ChargingPileEncoder;

namespace SHWD.ChargingPileBusiness.ProtocolEncoder
{
    public class StartChargingEncoder : FrameEncoderBase
    {
        private static byte[] CmdType => new byte[] { 0xF3 };

        private static byte[] CmdByte => new byte[] { 0x02 };

        private static byte[] OperateCode => new byte[] { 0x03 };

        private static byte[] ControlCode => new byte[] { 0x80, 0x00 };

        public override ChargingPileProtocolPackage Encode(string identity, Dictionary<string, string> pars)
        {
            if (!pars.ContainsKey("ShotIndentity")) return null;
            var package = new ChargingPileProtocolPackage
            {
                ["Head"] = new PackageComponent
                {
                    ComponentContent = new byte[] { 0x55 },
                    ComponentIndex = 0,
                    ComponentName = "Head"
                },
                ["CmdType"] = new PackageComponent
                {
                    ComponentContent = CmdType,
                    ComponentIndex = 1,
                    ComponentName = "CmdType"
                },
                ["CmdByte"] = new PackageComponent
                {
                    ComponentContent = CmdByte,
                    ComponentIndex = 2,
                    ComponentName = "CmdByte"
                },
                ["OperateCode"] = new PackageComponent
                {
                    ComponentContent = OperateCode,
                    ComponentIndex = 3,
                    ComponentName = "OperateCode"
                },
                ["ControlCode"] = new PackageComponent
                {
                    ComponentContent = ControlCode,
                    ComponentIndex = 4,
                    ComponentName = "ControlCode"
                },
            };

            GenerateRequestCode(package);

            package["ContentLength"] = new PackageComponent
            {
                ComponentContent = new byte[] { 0x06, 0x00 },
                ComponentIndex = 6,
                ComponentName = "ContentLength"
            };

            var shotIdentity = pars["ShotIndentity"];
            Console.WriteLine($"Identity:{identity}, ShotIdentity{shotIdentity}");
            var index = ClientSourceStatus.GetShotIndexByIdentity(identity, shotIdentity);
            if (index < 0) return null;

            package["Data"] = new PackageComponent
            {
                ComponentContent = new byte[] { (byte)(index + 1), 0x00, 0x00, 0x00, 0x01, 0x00, 0x01 },
                ComponentIndex = 7,
                ComponentName = "Data"
            };

            AddModbus(package);

            package["Tail"] = new PackageComponent
            {
                ComponentContent = new byte[] { 0xAA },
                ComponentIndex = 9,
                ComponentName = "Tail"
            };

            return package;
        }
    }
}
