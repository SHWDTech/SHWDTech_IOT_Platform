using SHWDTech.IOT.Storage.Communication.Entities;

namespace ProtocolCommunicationService.Coding
{
    public class DefaultClientSource : IClientSource
    {
        public DefaultClientSource(string identity, string nodeId, Business business)
        {
            ClientIdentity = identity;
            ClientNodeId = nodeId;
            Business = business;
        }

        public string ClientIdentity { get; }

        public string ClientNodeId { get; }

        public Business Business { get; }
    }
}
