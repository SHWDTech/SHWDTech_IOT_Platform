﻿namespace ProtocolCommunicationService.Core
{
    public delegate void ClientConnectedEventHandler(ClientConnectedEventArgs args);

    public delegate void ClientDisconnectedEventHandler(ClientDisconnectedEventArgs args);

    public delegate void DataReceived(ClientDataReceivedEventArgs args);

    public delegate void DataSend(ClientSendDataEventArgs args);
}
