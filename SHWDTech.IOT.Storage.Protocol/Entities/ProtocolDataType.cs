using System.ComponentModel.DataAnnotations;

namespace SHWDTech.IOT.Storage.Communication.Entities
{
    /// <summary>
    /// 数据包类型
    /// </summary>
    public enum ProtocolDataType : byte
    {
        /// <summary>
        /// 接收
        /// </summary>
        [Display(Name = "接收协议包")]
        Receive = 0x00,

        /// <summary>
        /// 发送
        /// </summary>
        [Display(Name = "发送协议包")]
        Send = 0x01
    }
}
