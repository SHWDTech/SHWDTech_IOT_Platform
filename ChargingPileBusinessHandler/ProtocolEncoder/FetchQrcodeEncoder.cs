using System;
using System.Collections.Generic;
using System.Text;
using ProtocolCommunicationService.Coding;
using SHWD.ChargingPileEncoder;

namespace SHWD.ChargingPileBusiness.ProtocolEncoder
{
    public class FetchQrcodeEncoder : FrameEncoderBase
    {
        private static byte[] CmdType => new byte[] { 0xF1 };

        private static byte[] CmdByte => new byte[] { 0x08 };

        private static byte[] OperateCode => new byte[] { 0x82 };

        private static byte[] ControlCode => new byte[] { 0x80, 0x00 };

        public override ChargingPileProtocolPackage Encode(string identity, Dictionary<string, string> pars)
        {
            if (!pars.ContainsKey("ShortIndex") || !pars.ContainsKey("Qrcode")) return null;
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

            var shotIndex = pars["ShortIndex"];
            var qrcode = pars["Qrcode"];
            var qrCodeBytes = Encoding.UTF8.GetBytes(qrcode);

            //充电枪序号
            var dataComponentBytes = new List<byte> {byte.Parse(shotIndex)};

            //二维码字符串长度
            var datalength = (short) qrCodeBytes.Length;
            var lengthBytes = BitConverter.GetBytes(datalength);
            Array.Reverse(lengthBytes);
            dataComponentBytes.AddRange(lengthBytes);

            //数据本体
            dataComponentBytes.AddRange(qrCodeBytes);

            package["Data"] = new PackageComponent
            {
                ComponentContent = dataComponentBytes.ToArray(),
                ComponentIndex = 7,
                ComponentName = "Data"
            };

            var totalLengthBytes = BitConverter.GetBytes((short) dataComponentBytes.Count);
            Array.Reverse(totalLengthBytes);

            package["ContentLength"] = new PackageComponent
            {
                ComponentContent = totalLengthBytes,
                ComponentIndex = 6,
                ComponentName = "ContentLength"
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
