using System.Collections.Generic;

namespace ProtocolCommunicationService.NetWorkCore
{
    public class BufferManager
    {
        private readonly List<byte> _buffer = new List<byte>();

        public int Count => _buffer.Count;

        public IReadOnlyCollection<byte> GetData => _buffer.AsReadOnly();

        public void Write(byte[] data, int datalens)
        {
            var index = 0;
            while (index < datalens)
            {
                _buffer.Add(data[index]);
                index++;
            }
        }

        public void Clear() => _buffer.Clear();

        public void RemoveAt(int index) => _buffer.RemoveAt(index);
    }
}
