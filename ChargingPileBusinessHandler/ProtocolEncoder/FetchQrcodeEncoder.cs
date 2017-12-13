using System;
using System.Collections.Generic;
using System.Text;
using ProtocolCommunicationService.Coding;
using SHWD.ChargingPileEncoder;

namespace SHWD.ChargingPileBusiness.ProtocolEncoder
{
    public class FetchQrcodeEncoder : FrameEncoderBase
    {
        private static byte[] CmdType => new byte[] { 0xF3 };

        private static byte[] CmdByte => new byte[] { 0x02 };

        private static byte[] OperateCode => new byte[] { 0x03 };

        private static byte[] ControlCode => new byte[] { 0x80, 0x00 };

        public override ChargingPileProtocolPackage Encode(string identity, Dictionary<string, string> pars)
        {
            if (!pars.ContainsKey("ShortIdentity") || !pars.ContainsKey("Qrcode")) return null;
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

            var shotIdentity = pars["ShortIdentity"];
            var qrcode = pars["Qrcode"];
            var index = ClientSourceStatus.GetShotIndexByIdentity(identity, shotIdentity);
            if (index < 0) return null;
            var datas = Encoding.UTF8.GetBytes(qrcode);
            var dataComponentBytes = new List<byte>();
            //对象数据编号
            dataComponentBytes.AddRange(new byte[] { (byte)(index + 1), 0x00 });

            //对象数据内容编号
            dataComponentBytes.AddRange(new byte[] { 0x15, 0x00 });

            //对象数据长度
            var datalength = (short) datas.Length;
            var lengthBytes = BitConverter.GetBytes(datalength);
            Array.Reverse(lengthBytes);
            dataComponentBytes.AddRange(lengthBytes);

            //数据本体
            dataComponentBytes.AddRange(datas);

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
