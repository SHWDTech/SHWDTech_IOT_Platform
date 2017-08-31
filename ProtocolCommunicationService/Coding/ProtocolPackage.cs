using System;
using System.Collections.Generic;
using System.Linq;
using SHWDTech.IOT.Storage.Communication.Entities;

namespace ProtocolCommunicationService.Coding
{
    public class ProtocolPackage : IProtocolPackage
    {
        public ProtocolPackage()
        {

        }

        public ProtocolPackage(ProtocolCommand command)
        {
            Protocol = command.Protocol;

            Command = command;

            foreach (var structure in Protocol.ProtocolStructures)
            {
                var component = new PackageComponent
                {
                    ComponentName = structure.StructureName,
                    ComponentIndex = structure.StructureIndex,
                    ComponentContent = structure.DefaultBytes
                };

                this[structure.StructureName] = component;
            }

            foreach (var commandData in command.CommandDatas)
            {
                var component = new PackageComponent
                {
                    ComponentName = commandData.DataName,
                    ComponentIndex = commandData.DataIndex
                };

                AppendData(component);
            }
        }
        public bool Finalized { get; protected set; }

        public virtual string PackageComponentFactors { get; } = string.Empty;

        public virtual int PackageByteLenth => StructureComponents.Select(s => s.Value.ComponentContent.Length).Sum() + DataComponent.ComponentContent.Length;

        public Dictionary<string, IPackageComponent> DataContentComponents { get; } = new Dictionary<string, IPackageComponent>();

        public IClientSource ClientSource { get; set; }

        public DateTime ReceiveDateTime { get; set; }
        public virtual byte[] CommandDefinitionCode => new byte[0];

        public ProtocolData ProtocolData { get; set; }

        public Protocol Protocol { get; set; }

        public ProtocolCommand Command { get; set; }

        public virtual int StructureComponentCount => StructureComponents.Count;

        public byte[] DeviceNodeId { get; set; }

        public int DataComponentIndex { get; protected set; }

        public List<string> DeliverParams => Command.DeliverParams;

        public PackageStatus Status { get; set; }

        public IPackageComponent this[string name]
        {
            get
            {
                if (name == "Data") return DataComponent;

                if (StructureComponents.ContainsKey(name)) return StructureComponents[name];

                return DataContentComponents.ContainsKey(name) ? DataContentComponents[name] : null;
            }
            set
            {
                if (name == "Data")
                {
                    SetDataComponent(value);
                    return;
                }

                if (!StructureComponents.ContainsKey(name))
                {
                    StructureComponents.Add(name, value);
                }
                else
                {
                    StructureComponents[name] = value;
                }
            }
        }

        protected virtual void SetDataComponent(IPackageComponent component)
        {
            DataComponent = component;
            DataComponentIndex = component.ComponentIndex;
        }

        public IPackageComponent DataComponent { get; protected set; }

        public Dictionary<string, IPackageComponent> StructureComponents { get; } = new Dictionary<string, IPackageComponent>();

        public void AppendData(IPackageComponent component)
        {
            DataContentComponents.Add(component.ComponentName, component);
        }

        public string GetDataValueString(string dataValueName)
        {
            return DataContentComponents.ContainsKey(dataValueName)
                ? DataContentComponents[dataValueName].ComponentStringValue
                : null;
        }

        public byte[] GetCrcBytes()
        {
            var allBytes = GetBytes();
            var finalBytes = new byte[allBytes.Length - 3];
            Array.Copy(allBytes, 0, finalBytes, 0, finalBytes.Length);
            return finalBytes;
        }

        public void SetupProtocolData()
        {
            if (ClientSource == null) return;
            ProtocolData = new ProtocolData
            {
                Business = ClientSource.Business,
                DeviceId = long.Parse(ClientSource.ClientIdentity),
                ProtocolContent = GetBytes(),
                ProtocolId = Protocol.Id,
                DecodeDateTime = DateTime.Now
            };
            ProtocolData.ContentLength = ProtocolData.ProtocolContent.Length;
        }

        public byte[] GetBytes()
        {
            var bytes = new List<byte>();

            for (var i = 0; i <= StructureComponents.Count; i++)
            {
                var componentBytes = i == DataComponentIndex
                    ? DataComponent.ComponentContent
                    : StructureComponents.First(obj => obj.Value.ComponentIndex == i).Value.ComponentContent;

                bytes.AddRange(componentBytes);
            }

            return bytes.ToArray();
        }

        public virtual void Finalization()
        {
            if (
                //数据段单独存放，因此_componentData的长度为协议结构长度减一
                StructureComponents.Count + 1 != Protocol.ProtocolStructures.Count
                || DataComponent == null
                || Command.DataOrderType == DataOrderType.Order && DataComponent.ComponentContent.Length != Command.ReceiveBytesLength
            )
            {
                Status = PackageStatus.InvalidPackage;
                return;
            }

            Status = PackageStatus.Finalized;

            Finalized = true;
        }
    }
}
