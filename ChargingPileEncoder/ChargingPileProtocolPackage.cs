﻿using System;
using System.Linq;
using System.Text;
using BasicUtility;
using ProtocolCommunicationService.Coding;

namespace ChargingPileEncoder
{
    public class ChargingPileProtocolPackage : ProtocolPackage
    {
        private byte[] _commandDefinitionCode;

        public byte CmdType => this[StructureNames.CmdType].ComponentContent[0];

        public byte CmdByte => this[StructureNames.CmdByte].ComponentContent[0];

        public override byte[] CommandDefinitionCode
        {
            get
            {
                if (_commandDefinitionCode != null) return _commandDefinitionCode;
                _commandDefinitionCode = new []{CmdType, CmdByte};
                return _commandDefinitionCode;
            }
        }

        private OperateCode _operateCode;

        public OperateCode OperateCode
        {
            get
            {
                if (_operateCode != null) return _operateCode;
                if (!StructureComponents.ContainsKey(nameof(OperateCode))) return null;
                _operateCode = new OperateCode(StructureComponents[nameof(OperateCode)].ComponentContent[0]);
                return _operateCode;
            }
        }

        private ControlCode _controlCode;

        public ControlCode ControlCode
        {
            get
            {
                if (_controlCode != null) return _controlCode;
                if (!StructureComponents.ContainsKey(nameof(ControlCode))) return null;
                _controlCode = new ControlCode(StructureComponents[nameof(ControlCode)].ComponentContent);
                return _controlCode;
            }
        }

        public override string PackageComponentFactors
        {
            get
            {
                if (!Finalized) return string.Empty;
                var sb = new StringBuilder();
                sb.AppendLine($"帧头：{StructureComponents[StructureNames.Head].ComponentContent.ToHexString()}。" +
                              $"指令类型：{StructureComponents[StructureNames.CmdType].ComponentContent.ToHexString()}。" +
                              $"指令符：{StructureComponents[StructureNames.CmdByte].ComponentContent.ToHexString()}。" +
                              $"操作码：0x{OperateCode.OperateByte:X2}，动作类型：{EnumHelper<Action>.GetDisplayValue(OperateCode.Action)}，操作类型：{EnumHelper<Operate>.GetDisplayValue(OperateCode.Operate)}。" +
                              $"控制符：0x{ControlCode.ControlBytes.ToHexString()}，是否需要回复：{ControlCode.NeedResponse}，异常代码：{ControlCode.ExceptionCode}，数据端口：{ControlCode.ResponsePorts}。" +
                              $"请求码：{StructureComponents[StructureNames.RequestCode].ComponentContent.ToHexString()}。" +
                              $"负载数据：{DataComponent.ComponentContent.ToHexString()}。" +
                              $"校验码：{StructureComponents[StructureNames.CrcModBus].ComponentContent.ToHexString()}。" +
                              $"帧尾：{StructureComponents[StructureNames.Tail].ComponentContent.ToHexString()}。\r\n" +
                              $"协议完整数据：{ProtocolData.ProtocolContent.ToHexString()}。");
                return sb.ToString();
            }
        }

        private static ushort GetCrcModBus(byte[] buffer)
        {
            ushort regCrc = 0xffff;

            var len = buffer.Length;

            for (var i = 0; i < len; i++)
            {
                regCrc ^= buffer[i];
                for (var j = 0; j < 8; j++)
                {
                    if ((regCrc & 0x0001) != 0)

                        regCrc = (ushort)(regCrc >> 1 ^ 0xA001);
                    else
                        regCrc >>= 1;
                }
            }
            var tempReg = (byte)(regCrc >> 8);

            return (ushort)(regCrc << 8 | tempReg);
        }

        public override void Finalization()
        {
            var crcBytes = BitConverter.GetBytes(GetCrcModBus(GetBytes()));
            Array.Reverse(crcBytes);
            if (!crcBytes.SequenceEqual(this["CrcModBus"].ComponentContent))
            {
                Status = PackageStatus.ValidationFailed;
                return;
            }
            base.Finalization();
        }
    }
}