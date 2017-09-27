using SHWDTech.IOT.Storage.Communication.Entities;

namespace ProtocolCommunicationService.Coding
{
    public class DefaultClientSource : IClientSource
    {
        public DefaultClientSource(string identity,string nodeIdStr, byte[] nodeId, Business business)
        {
            ClientIdentity = identity;
            ClientNodeIdString = nodeIdStr;
            ClientNodeId = nodeId;
            Business = business;
        }

        public string ClientIdentity { get; }

        public string ClientNodeIdString { get; }

        public byte[] ClientNodeId { get; }

        public Business Business { get; }
    }
}
