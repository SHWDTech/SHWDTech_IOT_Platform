﻿using System;
using System.Net.Sockets;
using BasicUtility;
using ProtocolCommunicationService.Coding;
using ProtocolCommunicationService.Core;
using SHWDTech.IOT.Storage.Communication.Entities;

namespace ProtocolCommunicationService.NetWorkCore
{
    public class DeviceClient : IDisposable
    {
        private readonly BufferManager _receiveBuffer;

        private AuthenticationStatus _authStatus = AuthenticationStatus.UnAuthenticated;

        public Socket ClientSocket { get; private set; }

        public Business Business { get; }

        public IClientSource ClientSource { get; set; }

        public event ClientDisconnectedEventHandler OnDisconnected;

        public event DataReceived OnDataReceived;

        public event DataSend OnDataSend;

        public event Authenticated OnAuthenticated;

        private readonly SocketAsyncEventArgs _asyncEventArgs;

        public DeviceClient(Socket clientSocket, Business business)
        {
            ClientSocket = clientSocket;
            Business = business;
            _receiveBuffer = new BufferManager();
            _asyncEventArgs = new SocketAsyncEventArgs();
            _asyncEventArgs.SetBuffer(new byte[4096], 0, 4096);
            _asyncEventArgs.Completed += (sender, args) =>
            {
                switch (args.LastOperation)
                {
                    case SocketAsyncOperation.Receive:
                        ProcessReceive();
                        break;
                    case SocketAsyncOperation.Send:
                        //DataSend(args.SendPacketsElements[0].Buffer);
                        break;
                }
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
            catch (Exception ex) when (ex is SocketException || ex is ObjectDisposedException)
            {
                Disconnect();
            }
        }

        public void Send(IProtocolPackage package)
        {
            Send(package.GetBytes());
        }

        private void ProcessReceive()
        {
            if (ClientSocket == null) return;
            try
            {
                if (_asyncEventArgs.BytesTransferred <= 0)
                {
                    Disconnect();
                    return;
                }
                if (_asyncEventArgs.SocketError == SocketError.Success)
                {
                    var receivedBytes = new byte[_asyncEventArgs.BytesTransferred];
                    Array.Copy(_asyncEventArgs.Buffer, _asyncEventArgs.Offset, receivedBytes, 0,
                        _asyncEventArgs.BytesTransferred);
                    _receiveBuffer.AddRange(receivedBytes);
                    DataReceived(receivedBytes);

                    Decode();
                }
                var willRaiseEvent = ClientSocket.ReceiveAsync(_asyncEventArgs);
                if (!willRaiseEvent)
                {
                    ProcessReceive();
                }
            }
            catch (Exception ex) when (ex is SocketException || ex is ObjectDisposedException)
            {
                Disconnect();
            }
        }

        private void Decode()
        {
            try
            {
                var package = EncoderManager.Decode(_receiveBuffer.ToArray());
                package.ClientSource = ClientSource;
                CleanBuffer(package);
                if (package.Status == PackageStatus.InvalidHead && _receiveBuffer.Count > 0)
                {
                    Decode();
                }
                if (_authStatus == AuthenticationStatus.UnAuthenticated)
                {
                    Authentication(package);
                }
            }
            catch (Exception ex)
            {
                LogService.Instance.Error("decode protocol failed", ex);
            }
        }

        private void CleanBuffer(IProtocolPackage package)
        {
            if (package == null) return;
            switch (package.Status)
            {
                case PackageStatus.InvalidHead:
                    _receiveBuffer.RemoveHead();
                    return;
                case PackageStatus.InvalidPackage:
                    _receiveBuffer.RemoveRange(0, package.PackageByteLenth);
                    return;
                case PackageStatus.Finalized:
                    _receiveBuffer.RemoveRange(0, package.PackageByteLenth);
                    return;
                case PackageStatus.ValidationFailed:
                    _receiveBuffer.RemoveHead();
                    return;
            }
        }

        private void Authentication(IProtocolPackage package)
        {
            var result = EncoderManager.Authentication(package, Business);
            _authStatus = result.Status;
            switch (_authStatus)
            {
                case AuthenticationStatus.Authenticated:
                    Authenticated(result.ClientSource);
                    break;
                case AuthenticationStatus.DeviceNotRegisted:
                    Disconnect();
                    break;
            }
        }

        private void Disconnect()
        {
            if (ClientSocket == null) return;
            ClientSocket.Close();
            Disconnected();
        }

        private void DataReceived(byte[] buffer)
        {
            OnDataReceived?.Invoke(new ClientDataReceivedEventArgs(ClientSocket, buffer, buffer.Length));
        }

        private void DataSend(byte[] sendBytes)
        {
            OnDataSend?.Invoke(new ClientSendDataEventArgs(ClientSocket, sendBytes));
        }

        private void Disconnected()
        {
            OnDisconnected?.Invoke(new ClientDisconnectedEventArgs());
            Dispose();
        }

        private void CleanUp()
        {
            OnDataReceived = null;
            OnDataSend = null;
            OnDisconnected = null;
            OnAuthenticated = null;
            ClientSource = null;
        }

        private void Authenticated(IClientSource source)
        {
            OnAuthenticated?.Invoke(new ClientAuthenticatedArgs(this, source, Business));
        }

        ~DeviceClient()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (ClientSocket == null) return;
            CleanUp();
            _asyncEventArgs?.Dispose();
            ClientSocket.Dispose();
            ClientSocket = null;
            ConnectedClients.Instance.Remove(this);
            GC.SuppressFinalize(this);
        }
    }
}
