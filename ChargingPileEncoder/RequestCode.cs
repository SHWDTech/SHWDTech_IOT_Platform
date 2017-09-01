using System;

namespace SHWD.ChargingPileEncoder
{
    public class RequestCode
    {
        public RequestCode(byte[] codeBytes)
        {
            if(codeBytes.Length != 8) throw new ArgumentException("RequestCode should be eight bytes");
            Year = codeBytes[0];
            Month = codeBytes[1];
            Day = codeBytes[2];
            Hour = codeBytes[3];
            Minute = codeBytes[4];
            Second = codeBytes[5];
            Millisecond = codeBytes[6] + codeBytes[7] << 8;
        }

        public string RequestCodeStr => $"{Year}{Month}{Day}{Hour}{Minute}{Second}{Millisecond}";

        public byte Year { get; }

        public byte Month { get; }

        public byte Day { get; }

        public byte Hour { get; }

        public byte Minute { get; }

        public byte Second { get; }

        public int Millisecond { get; }
    }
}
