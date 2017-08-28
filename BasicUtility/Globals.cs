using System;
using System.IO;

namespace BasicUtility
{
    public static class Globals
    {
        private static string _applicationPath = string.Empty;

        /// <summary>
        /// 返回应用程序启动路径
        /// </summary>
        public static string ApplicationPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_applicationPath))
                {
                    _applicationPath = AppDomain.CurrentDomain.BaseDirectory;

                    _applicationPath = _applicationPath.Replace("/", Path.DirectorySeparatorChar.ToString());

                    _applicationPath = _applicationPath.TrimEnd(Path.DirectorySeparatorChar);
                }

                return _applicationPath;
            }
        }

        /// <summary>
        ///  bytes转INT16
        /// </summary>
        /// <param name="bufferIndex"></param>
        /// <param name="isLittleEndian"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static short BytesToInt16(byte[] buffer, int bufferIndex, bool isLittleEndian)
        {
            short val;

            if (isLittleEndian)
            {
                val = (short)((buffer[bufferIndex] << 8) + buffer[bufferIndex + 1]);
            }
            else
            {
                val = (short)((buffer[bufferIndex + 1] << 8) + buffer[bufferIndex]);
            }

            return val;
        }

        /// <summary>
        ///  bytes转UINT64
        /// </summary>
        /// <param name="bufferIndex"></param>
        /// <param name="isLittleEndian"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static ulong BytesToUint64(byte[] buffer, int bufferIndex, bool isLittleEndian)
        {
            ulong val;

            if (isLittleEndian)
            {
                val = ((ulong)((buffer[bufferIndex + 7] << 56) + (buffer[bufferIndex + 6] << 48)
                               + (buffer[bufferIndex + 5] << 40) + (buffer[bufferIndex + 4] << 32)
                               + (buffer[bufferIndex + 3] << 24) + (buffer[bufferIndex + 2] << 16)
                               + (buffer[bufferIndex + 1] << 8) + buffer[bufferIndex])) & 0xFFFFFFFFFFFFFFFF;
            }
            else
            {
                val = ((ulong)((buffer[bufferIndex] << 56) + (buffer[bufferIndex + 1] << 48)
                               + (buffer[bufferIndex + 2] << 40) + (buffer[bufferIndex + 3] << 32)
                               + (buffer[bufferIndex + 4] << 24) + (buffer[bufferIndex + 5] << 16)
                               + (buffer[bufferIndex + 6] << 8) + buffer[bufferIndex + 7])) & 0xFFFFFFFFFFFFFFFF;
            }

            return val;
        }
    }
}
