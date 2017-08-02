using System.Net;
using System.Net.Sockets;

namespace ProtocolCommunicationService.NetWorkCore
{
    public class DeviceListener
    {
        private Socket _linstenSocket;

        public void StartListen()
        {
            _linstenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
    }
}
