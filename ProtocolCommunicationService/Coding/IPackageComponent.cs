namespace ProtocolCommunicationService.Coding
{
    /// <summary>
    /// 协议包组件
    /// </summary>
    public interface IPackageComponent
    {
        /// <summary>
        /// 组件名称
        /// </summary>
        string ComponentName { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        string DataType { get; set; }

        /// <summary>
        /// 数据索引
        /// </summary>
        int ComponentIndex { get; set; }

        /// <summary>
        /// 数据有效性验证位
        /// </summary>
        byte ValidFlag { get; set; }

        /// <summary>
        /// 组件数据
        /// </summary>
        byte[] ComponentContent { get; set; }

        /// <summary>
        /// 组件数据值
        /// </summary>
        string ComponentStringValue { get; }
    }
}
