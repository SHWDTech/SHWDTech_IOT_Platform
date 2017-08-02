
// ReSharper disable InconsistentNaming

namespace SHWDTech.IOT.Storage.Communication
{
    public enum EncryptType : byte
    {
        None = 0x00,

        MD5 = 0x01,

        RSA = 0x02,

        DES = 0x03
    }
}
