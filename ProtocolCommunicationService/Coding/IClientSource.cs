using SHWDTech.IOT.Storage.Communication.Entities;

namespace ProtocolCommunicationService.Coding
{
    public interface IClientSource
    {
        /// <summary>
        /// 数据源身份码
        /// </summary>
        string ClientIdentity { get; }

        /// <summary>
        /// 客户端NODEID
        /// </summary>
        string ClientNodeId { get; }

        /// <summary>
        /// 数据源所属业务
        /// </summary>
        Business Business { get; }
    }
}
