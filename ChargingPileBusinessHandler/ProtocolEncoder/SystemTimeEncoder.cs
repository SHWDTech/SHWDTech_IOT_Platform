using System;
using System.Collections.Generic;
using ProtocolCommunicationService.Coding;
using SHWD.ChargingPileEncoder;

namespace SHWD.ChargingPileBusiness.ProtocolEncoder
{
    public class SystemTimeEncoder : FrameEncoderBase
    {
        private static byte[] CmdType => new byte[] { 0xF1 };

        private static byte[] CmdByte => new byte[] { 0x04 };

        private static byte[] OperateCode => new byte[] { 0x00 };

        private static byte[] ControlCode => new byte[] { 0x80, 0x00 };

        public override ChargingPileProtocolPackage Encode(string identity, Dictionary<string, string> pars)
        {
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
                ComponentContent = new byte[] { 0x08, 0x00 },
                ComponentIndex = 6,
                ComponentName = "ContentLength"
            };

            var now = DateTime.Now;
            var millsecond = (short)now.Millisecond;
            var msBytes = BitConverter.GetBytes(millsecond);

            package["Data"] = new PackageComponent
            {
                ComponentContent = new []
                {
                    (byte)(now.Year - 2000),
                    (byte)now.Month,
                    (byte)now.Day,
                    (byte)now.Hour,
                    (byte)now.Minute,
                    (byte)now.Second,
                    msBytes[0],
                    msBytes[1]
                },
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
