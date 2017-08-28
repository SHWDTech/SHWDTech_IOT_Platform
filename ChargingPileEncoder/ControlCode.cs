namespace ChargingPileEncoder
{
    public class ControlCode
    {
        public ControlCode(byte[] code)
        {
            ControlBytes = code;
            var value = (ushort)((code[1] << 8) + code[0]);
            NeedResponse = (value & (1 << 0xF)) == 1;
            ExceptionCode = (value & 0x7F00) >> 8;
            ResponsePorts = value & 0xFF;
        }

        public bool NeedResponse { get; }

        public int ExceptionCode { get; }

        public int ResponsePorts { get; }

        public byte[] ControlBytes { get; }
    }
}
