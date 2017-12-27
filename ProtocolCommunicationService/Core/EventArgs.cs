using System;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using ProtocolCommunicationService.Coding;
using ProtocolCommunicationService.NetWorkCore;
using SHWDTech.IOT.Storage.Communication.Entities;

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
        public string RemoteEndPoint { get; set; }

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

        public Business Business { get; }

        public DeviceClient Client { get; }

        public ClientSendDataEventArgs(Socket targetSocket) : base(targetSocket)
        {

        }

        public ClientSendDataEventArgs(DeviceClient deviceClient, byte[] buffer) : this(deviceClient.ClientSocket)
        {
            Client = deviceClient;
            SendContent = Array.AsReadOnly(buffer);
        }

        public ClientSendDataEventArgs(DeviceClient deviceClient, byte[] buffer, Business business) : this(deviceClient, buffer)
        {
            SendContent = Array.AsReadOnly(buffer);
            Business = business;
        }
    }

    public class ClientAuthenticatedArgs : TcpClientEventArgs
    {
        public IClientSource AuthenticatedClientSource { get; }

        public DeviceClient Client { get; }

        public Business Business { get; }

        public ClientAuthenticatedArgs(DeviceClient client, IClientSource clientSource, Business business)
        {
            Client = client;
            AuthenticatedClientSource = clientSource;
            Business = business;
        }
    }

    public class ClientDecodeSucessEventArgs : TcpClientEventArgs
    {
        public IProtocolPackage DecodedPackage { get; }

        public Business Business { get; }

        public ClientDecodeSucessEventArgs(IProtocolPackage package, Business business)
        {
            DecodedPackage = package;
            Business = business;
        }
    }

    public class ClientDecodeFinishedEventArgs : TcpClientEventArgs
    {
        public IProtocolPackage DecodedPackage { get; }

        public Business Business { get; }

        public ClientDecodeFinishedEventArgs(IProtocolPackage package, Business business)
        {
            DecodedPackage = package;
            Business = business;
        }
    }
}
