using BasicUtility;

namespace SHWD.ChargingPileEncoder
{
    public class ChargingPilePackageDataObject
    {
        public ushort Target { get; }

        public ushort DataContentType { get; }

        public ushort DataContentLength { get; }

        public byte[] DataBytes { get; }

        public ChargingPilePackageDataObject(byte[] data, ushort dataLength)
        {
            Target = (ushort) (data[1] << 8 + data[0]);
            DataContentType = (ushort) (data[3] << 8 + data[2]);
            DataContentLength = dataLength;
            DataBytes = data.SubBytes(6, dataLength);
        }
    }

    public enum DataTarget : ushort
    {
        Server = 0x00,

        ChargingPile = 0x01
    }
}
