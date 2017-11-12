using System;
using System.Linq;
using BasicUtility;
using ProtocolCommunicationService.Coding;
using SHWDTech.IOT.Storage.Communication.Entities;

namespace SHWD.ChargingPileEncoder
{
    public class ChargingPileEncoder : IProtocolEncoder
    {
        public Protocol Protocol { get; set; }

        public IProtocolPackage Decode(byte[] protocolBytes)
        {
            var package = new ChargingPileProtocolPackage { Protocol = Protocol, ReceiveDateTime = DateTime.Now };

            var structures = Protocol.ProtocolStructures.ToList();

            var currentIndex = 0;

            for (var i = 0; i < structures.Count; i++)
            {
                var structure = structures.First(obj => obj.StructureIndex == i);

                var componentDataLength = structure.StructureName == StructureNames.Data && structure.StructureDataLength == 0
                    ? Globals.BytesToInt16(package["ContentLength"].ComponentContent, 0, true)
                    : structure.StructureDataLength;

                if (currentIndex + componentDataLength > protocolBytes.Length)
                {
                    package.Status = PackageStatus.NoEnoughBuffer;
                    package.AddDecodeError("缓存数据长度不足。");
                    return package;
                }

                if (structure.StructureName == StructureNames.Data)
                {
                    DetectCommand(package);
                    if (package.Command == null)
                    {
                        package.Status = PackageStatus.InvalidCommand;
                        package.AddDecodeError("指令类型或指令码不正确。");
                        return package;
                    }
                    componentDataLength = package.Command.ReceiveBytesLength == 0 ? componentDataLength : package.Command.ReceiveBytesLength;
                }

                var component = new PackageComponent
                {
                    ComponentName = structure.StructureName,
                    ComponentIndex = structure.StructureIndex,
                    ComponentContent = protocolBytes.SubBytes(currentIndex, currentIndex + componentDataLength)
                };

                currentIndex += componentDataLength;

                package[structure.StructureName] = component;
            }

            DecodeCommand(package);

            return package;
        }

        protected static void DecodeCommand(ChargingPileProtocolPackage package)
        {
            var currentIndex = 0;

            var container = package[StructureNames.Data].ComponentContent;

            for (var i = 0; i < package.Command.CommandDatas.Count; i++)
            {
                var data = package.Command.CommandDatas.First(obj => obj.DataIndex == i);

                if (currentIndex + data.DataLength > container.Length)
                {
                    package.Status = PackageStatus.NoEnoughBuffer;
                    package.AddDecodeError("缓存数据长度不足。");
                    return;
                }

                var component = new PackageComponent
                {
                    ComponentName = data.DataName,
                    ComponentIndex = data.DataIndex,
                    ComponentContent = container.SubBytes(currentIndex, currentIndex + data.DataLength)
                };

                currentIndex += data.DataLength;

                package.AppendData(component);
            }
            if (package.CmdType == 0xF1 && package.CmdByte == 0x06)
            {
                package.DeviceNodeId = package[StructureNames.Data].ComponentContent;
            }

            if (package.Command.CommandCategory == "HeartBeat")
            {
                package.ValidAsHeartBeat();
            }

            package.Finalization();
        }

        protected void DetectCommand(ChargingPileProtocolPackage package)
        {
            foreach (var command in Protocol.ProtocolCommands.Where(command =>
                package.CommandDefinitionCode.SequenceEqual(command.CommandCode)))
            {
                package.Command = command;
            }
        }
    }
}
