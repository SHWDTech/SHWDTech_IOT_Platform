using BasicUtility;

namespace SHWD.ChargingPileEncoder
{
    public class ChargingPilePackageDataObject
    {
        /// <summary>
        /// 数据包指向的目标，0表示充电桩，其余表示充电枪
        /// </summary>
        public ushort Target { get; }

        /// <summary>
        /// 数据包的类型
        /// </summary>
        public int DataContentType { get; }

        public ushort DataContentLength { get; }

        public byte[] DataBytes { get; }

        public ChargingPilePackageDataObject(byte[] data, ushort dataLength)
        {
            Target = (ushort) ((data[1] << 8) + data[0]);
            DataContentType = (data[3] << 8) + data[2];
            DataContentLength = dataLength;
            DataBytes = data.SubBytes(6, 6 + dataLength);
        }
    }

    public enum DataTarget : ushort
    {
        Server = 0x00,

        ChargingPile = 0x01
    }

    public enum ChargingPileDataType
    {
        SelfTest = 0x00
    }

    public enum RechargeShotDataType
    {
        ChargingAmount = 0x05,

        StartCharging = 0x06,

        StopCharging = 0x07,

        FetchRechrogeShotQrcode = 0x15
    }
}
