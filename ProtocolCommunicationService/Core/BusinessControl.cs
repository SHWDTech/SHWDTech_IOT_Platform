using System;
using System.Collections.Generic;
using System.Linq;
using ProtocolCommunicationService.Coding;
using ProtocolCommunicationService.NetWorkCore;
using ProtocolCommunicationService.Schedule;
using Quartz;
using Quartz.Impl;
using SHWDTech.IOT.Storage.Communication.Entities;

namespace ProtocolCommunicationService.Core
{
    public class BusinessControl
    {
        private DeviceListener _deviceListener;

        private readonly Dictionary<string, IOTDevice> _iotDevices = new Dictionary<string, IOTDevice>();

        public Business Business { get; }

        public bool IsListening => _deviceListener.IsListening;

        private static readonly IScheduler Scheduler;

        private IBusinessHandler _businessHandler;

        static BusinessControl()
        {
            Scheduler = StdSchedulerFactory.GetDefaultScheduler();
        }

        public BusinessControl(Business business)
        {
            Business = business;
            SetUpBusiness();
        }

        private void SetUpBusiness()
        {
            SetupListener();
            SetupHandler();
        }

        private void SetupListener()
        {
            _deviceListener = new DeviceListener(Business);
            _deviceListener.OnClientConnected += ClientConnected;
            _deviceListener.OnClientDisconnected += ClientDisconnected;
            _deviceListener.OnClientAuthenticated += ClientAuthenticated;
            _deviceListener.OnClientPackageDecodeSuccessed += ClientPackageDecodeSuccessed;
            _deviceListener.OnClientDataSend += ClientDataSend;
        }

        private void SetupHandler()
        {
            _businessHandler = EncoderManager.BusinessHandlers[Business.Id];
        }

        public void Start()
        {
            _deviceListener.StartListen(ServiceControl.ServerPublicIpAddress, Business.Port);
            var job = JobBuilder.Create<ClientDeviceConnecCheckJob>().Build();
            job.JobDataMap.Add("control", this);
            StartSchedule(job);
        }

        private static void StartSchedule(IJobDetail job)
        {
            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever())
                .Build();

            Scheduler.ScheduleJob(job, trigger);
        }

        private void ClientConnected(ClientConnectedEventArgs args)
        {
            ServiceControl.Instance.ClientConnected(args, this);
        }

        private void ClientDisconnected(ClientDisconnectedEventArgs args)
        {
            ServiceControl.Instance.ClientDisconnected(args, this);
        }

        private void ClientAuthenticated(ClientAuthenticatedArgs args)
        {
            ServiceControl.Instance.ClientAuthenticated(args, this);
            _businessHandler.ClientAuthenticated(args);
        }

        private void ClientDataSend(ClientSendDataEventArgs args)
        {
            ServiceControl.Instance.ClientDataSend(args, this);
        }

        private void ClientPackageDecodeSuccessed(ClientDecodeSucessEventArgs args)
        {
            EncoderManager.BusinessHandlers[Business.Id]?.OnPackageReceive(args);
        }

        public IOTDevice LookUpIotDevice(string nodeid)
        {
            if (string.IsNullOrWhiteSpace(nodeid)) return null;
            return _iotDevices.ContainsKey(nodeid) ? _iotDevices[nodeid] : null;
        }

        public void AppendIotDevice(IOTDevice device)
        {
            if (_iotDevices.ContainsKey(device.NodeIdString))
            {
                _iotDevices[device.NodeIdString] = device;
                return;
            }
            _iotDevices.Add(device.NodeIdString, device);
        }

        public void CheckDeviceConnection()
        {
            var misConnectDevices =
                _iotDevices.Where(dev => DateTime.Now - dev.Value.ReceiveDataTime > TimeSpan.FromMinutes(4));
            foreach (var device in misConnectDevices)
            {
                device.Value.DeviceClient.Disconnect();
            }
        }
    }
}
