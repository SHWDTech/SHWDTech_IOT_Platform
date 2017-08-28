using System;
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

        /// <summary>
        /// 返回位于指定起始位置和结束位置之间的字节数组
        /// </summary>
        /// <param name="sourceBytes">源字节数组</param>
        /// <param name="startIndex">返回字节集合的起始位置</param>
        /// <param name="endIndex">返回字节集合的结束位置</param>
        /// <returns>子字节数组</returns>
        public static byte[] SubBytes(this byte[] sourceBytes, int startIndex, int endIndex)
        {
            if (endIndex > sourceBytes.Length) throw new ArgumentOutOfRangeException();

            if (startIndex > endIndex) throw new ArgumentException("起始位置必须小于结束位置");

            if (startIndex == endIndex) return new byte[0];

            var returnBytes = new byte[endIndex - startIndex];

            for (var i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = sourceBytes[startIndex + i];
            }

            return returnBytes;
        }
    }
}
