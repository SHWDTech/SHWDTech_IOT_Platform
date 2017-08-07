using System;
using System.Net;
using System.Net.Sockets;
using ProtocolCommunicationService.Core;

namespace ProtocolCommunicationService.NetWorkCore
{
    public class DeviceListener
    {
        private Socket _listenSocket;

        private readonly int _port;

        public bool IsListening { get; private set; }

        public event ClientConnectedEventHandler OnClientConnected;

        public event ClientDisconnectedEventHandler OnClientDisconnected;

        public DeviceListener(int port)
        {
            _port = port;
        }

        public void StartListen()
        {
            if (IsListening) return;
            PrepareSocket();
            _listenSocket.Listen(4096);
            IsListening = true;
        }

        private void PrepareSocket()
        {
            _listenSocket?.Dispose();
            _listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listenSocket.Bind(new IPEndPoint(ServiceControl.ServerPublicIpAddress, _port));
            _listenSocket.LingerState = new LingerOption(false, 1);
        }

        private void ClientConnected(ClientConnectedEventArgs args)
        {
            args?.SetSourceSocket(_listenSocket);
            OnClientConnected?.Invoke(args);
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
                var client = new DeviceClient(acceptEventArgs.AcceptSocket);
                client.OnDisconnected += ClientDisconnected;
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
    }
}
