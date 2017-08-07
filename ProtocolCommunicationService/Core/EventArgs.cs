using System;
using System.Collections.ObjectModel;
using System.Net.Sockets;

namespace ProtocolCommunicationService.Core
{
    public class TcpClientEventArgs : EventArgs
    {
        public TcpClientEventArgs()
        {
            
        }

        public TcpClientEventArgs(Socket targetSocket) : this()
        {
            TargetSocket = targetSocket;
        }

        public void SetSourceSocket(Socket sourceSocket)
        {
            if (SourceSocket != null) SourceSocket = sourceSocket;
        }

        public Socket SourceSocket { get; private set; }

        public Socket TargetSocket { get; }
    }

    public class ClientConnectedEventArgs : TcpClientEventArgs
    {
        public ClientConnectedEventArgs()
        {
            
        }

        public ClientConnectedEventArgs(Socket targetSocket) : base(targetSocket)
        {
            
        }
    }

    public class ClientDisconnectedEventArgs : TcpClientEventArgs
    {
        public ClientDisconnectedEventArgs()
        {

        }

        public ClientDisconnectedEventArgs(Socket targetSocket) : base(targetSocket)
        {

        }
    }

    public class ClientDataReceivedEventArgs : TcpClientEventArgs
    {
        public ReadOnlyCollection<byte> Buffer { get; }

        public int DataLength { get; }

        public ClientDataReceivedEventArgs()
        {

        }

        public ClientDataReceivedEventArgs(Socket targetSocket) : base(targetSocket)
        {

        }

        public ClientDataReceivedEventArgs(Socket targetSocket, byte[] buffer, int dataLength) : this(targetSocket)
        {
            Buffer = Array.AsReadOnly(buffer);
            DataLength = dataLength;
        }
    }

    public class ClientSendDataEventArgs : TcpClientEventArgs
    {
        public ReadOnlyCollection<byte> SendContent { get; }

        public ClientSendDataEventArgs()
        {

        }

        public ClientSendDataEventArgs(Socket targetSocket) : base(targetSocket)
        {

        }

        public ClientSendDataEventArgs(Socket targetSocket, byte[] buffer) : this(targetSocket)
        {
            SendContent = Array.AsReadOnly(buffer);
        }
    }
}
