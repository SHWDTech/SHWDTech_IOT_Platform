
// ReSharper disable InconsistentNaming

using System;
using SHWDTech.IOT.Storage.Communication;

namespace ProtocolCommunicationService.Core
{
    public class IOTDevice
    {
        private readonly Device _device;

        public string Name => _device.DeviceName;

        public string NodeIdHexString => _device.NodeIdHexString;
        

        public IOTDevice(Device dev)
        {
            _device = dev;
        }
    }
}
