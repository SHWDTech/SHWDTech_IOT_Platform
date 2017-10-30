namespace SHWD.ChargingPileEncoder
{
    public enum SystemCommand
    {
        /// <summary>
        /// 软硬件版本读取
        /// </summary>
        ReadFirmwareVersion = 0x01,

        /// <summary>
        /// 读取设备身份ID
        /// </summary>
        ReadNodeId = 0x02,

        /// <summary>
        /// 软件复位
        /// </summary>
        SoftwareReset = 0x03,

        /// <summary>
        /// RTC时间
        /// </summary>
        RtcTime = 0x04,

        /// <summary>
        /// GPS坐标
        /// </summary>
        GpsCordinate = 0x05,
            
        /// <summary>
        /// 心跳包
        /// </summary>
        HeartBeat = 0x06
    }
}
