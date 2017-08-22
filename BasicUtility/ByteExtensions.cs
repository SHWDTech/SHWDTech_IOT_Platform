using System.Text;

namespace BasicUtility
{
    public static class ByteExtensions
    {
        public static string ToHexString(this byte[] data)
        {
            var builder = new StringBuilder();
            foreach (var t in data)
            {
                builder.Append($"{t:X2}".PadRight(3, ' '));
            }
            return builder.ToString();
        }
    }
}
