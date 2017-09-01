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

        public void AddRange(IEnumerable<byte> data)
        {
            _buffer.AddRange(data);
        }

        public byte[] ToArray() => _buffer.ToArray();

        public void RemoveHead()
        {
            if (_buffer.Count > 0)
                _buffer.RemoveAt(0);
        }

        public void RemoveRange(int index, int length)
        {
            if (_buffer.Count > length)
            {
                _buffer.RemoveRange(index, length);
            }
            else
            {
                _buffer.Clear();
            }
        }

        public void Clear() => _buffer.Clear();

        public void RemoveAt(int index) => _buffer.RemoveAt(index);
    }
}
