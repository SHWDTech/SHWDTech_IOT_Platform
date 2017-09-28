using System;
using System.Net;
using System.Net.Sockets;
using ProtocolCommunicationService.Core;
using SHWDTech.IOT.Storage.Communication.Entities;

namespace ProtocolCommunicationService.NetWorkCore
{
    public class DeviceListener
    {
        private Socket _listenSocket;

        private readonly Business _business;

        public bool IsListening { get; private set; }

        public event ClientConnectedEventHandler OnClientConnected;

        public event ClientDisconnectedEventHandler OnClientDisconnected;

        public event Authenticated OnClientAuthenticated;

        public event DecodeSuccessEventHandler OnClientPackageDecodeSuccessed;

        public event DataSend OnClientDataSend;

        public DeviceListener(Business business)
        {
            _business = business;
        }

        public void StartListen(IPAddress address, int port)
        {
            if (IsListening) return;
            PrepareSocket(address, port);
            _listenSocket.Listen(4096);
            StartAccept(null);
            IsListening = true;
        }

        private void PrepareSocket(IPAddress address, int port)
        {
            _listenSocket?.Dispose();
            _listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listenSocket.Bind(new IPEndPoint(address, port));
            _listenSocket.LingerState = new LingerOption(false, 1);
        }

        private void ClientConnected(ClientConnectedEventArgs args)
        {
            args?.SetSourceSocket(_listenSocket);
            OnClientConnected?.Invoke(args);
        }

        private void ClientAuthticated(ClientAuthenticatedArgs args)
        {
            OnClientAuthenticated?.Invoke(args);
        }

        private void ClientDataSend(ClientSendDataEventArgs args)
        {
            OnClientDataSend?.Invoke(args);
        }

        private void StartAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            if (_listenSocket == null) return;
            if (acceptEventArgs == null)
            {
                acceptEventArgs = new SocketAsyncEventArgs();
                acceptEventArgs.Completed += AcceptEventCompleted;
            }
            else
            {
                acceptEventArgs.AcceptSocket = null; //释放上次绑定的Socket，等待下一个Socket连接
            }

            var willRaiseEvent = _listenSocket.AcceptAsync(acceptEventArgs);//同步才是false，大多数的情况下都是异步的
            if (!willRaiseEvent)
            {
                ProcessAccept(acceptEventArgs);
            }
        }

        private void AcceptEventCompleted(object sender, SocketAsyncEventArgs acceptEventArgs)
        {
            ProcessAccept(acceptEventArgs);
        }

        private void ProcessAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            try
            {
                var client = new DeviceClient(acceptEventArgs.AcceptSocket, _business);
                client.OnDisconnected += ClientDisconnected;
                client.OnAuthenticated += ClientAuthticated;
                client.OnPackageDecodedSuccessed += ClientPackageDecodeSuccessed;
                client.OnDataSend += ClientDataSend;
                ConnectedClients.Instance.Append(client);
                ClientConnected(new ClientConnectedEventArgs(acceptEventArgs.AcceptSocket));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            StartAccept(acceptEventArgs); //把当前异步事件释放，等待下次连接
        }

        private void ClientDisconnected(ClientDisconnectedEventArgs args)
        {
            OnClientDisconnected?.Invoke(args);
        }

        private void ClientPackageDecodeSuccessed(ClientDecodeSucessEventArgs args)
        {
            OnClientPackageDecodeSuccessed?.Invoke(args);
        }
    }
}
