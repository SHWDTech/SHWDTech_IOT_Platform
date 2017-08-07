using System;
using System.Net.Sockets;
using ProtocolCommunicationService.Core;

namespace ProtocolCommunicationService.NetWorkCore
{
    public class DeviceClient : IDisposable
    {
        public Socket ClientSocket { get; private set; }

        public event ClientDisconnectedEventHandler OnDisconnected;

        public event DataReceived OnDataReceived;

        public event DataSend OnDataSend;

        private readonly SocketAsyncEventArgs _asyncEventArgs;

        public DeviceClient(Socket clientSocket)
        {
            ClientSocket = clientSocket;
            _asyncEventArgs = new SocketAsyncEventArgs();
            _asyncEventArgs.SetBuffer(new byte[4096], 0, 4096);
            _asyncEventArgs.Completed += (sender, args) =>
            {
                ProcessReceive();
            };
            var willRaiseEvent = ClientSocket.ReceiveAsync(_asyncEventArgs);
            if (willRaiseEvent) return;
            lock (ClientSocket)
            {
                ProcessReceive();
            }
        }

        public void Send(byte[] sendBytes)
        {
            try
            {
                ClientSocket.Send(sendBytes);
                DataSend(sendBytes);
            }
            catch (Exception ex) when (ex is SocketException)
            {
                Disconnect();
            }
        }

        private void ProcessReceive()
        {
            if (ClientSocket == null) return;
            try
            {
                if (_asyncEventArgs.BytesTransferred <= 0)
                {
                    Disconnect();
                }
                if (_asyncEventArgs.SocketError == SocketError.Success)
                {
                    DataReceived(_asyncEventArgs.Buffer, _asyncEventArgs.BytesTransferred);
                }
                var willRaiseEvent = ClientSocket.ReceiveAsync(_asyncEventArgs);
                if (willRaiseEvent) return;
                ProcessReceive();
            }
            catch (Exception ex) when (ex is SocketException)
            {
                Disconnect();
            }
        }

        private void Disconnect()
        {
            if (ClientSocket == null) return;
            lock (ClientSocket)
            {
                ClientSocket?.Dispose();
                ClientSocket = null;
                Disconnected();
            }
        }

        private void DataReceived(byte[] buffer, int dataLength)
        {
            OnDataReceived?.Invoke(new ClientDataReceivedEventArgs(ClientSocket, buffer, dataLength));
        }

        private void DataSend(byte[] sendBytes)
        {
            OnDataSend?.Invoke(new ClientSendDataEventArgs(ClientSocket, sendBytes));
        }

        private void Disconnected()
        {
            OnDisconnected?.Invoke(new ClientDisconnectedEventArgs(ClientSocket));
        }

        public void Dispose()
        {
            _asyncEventArgs?.Dispose();
            ClientSocket?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
