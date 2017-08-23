using SHWDTech.IOT.Storage.Communication.Entities;

namespace ProtocolCommunicationService.Coding
{
    public interface IProtocolEncoder
    {
        Protocol Protocol { get; set; }

        IProtocolPackage Decode(byte[] protocolBytes);
    }
}
