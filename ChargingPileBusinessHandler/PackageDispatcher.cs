﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProtocolCommunicationService.Core;
using SHWD.ChargingPileBusiness.ProtocolEncoder;
using SHWD.ChargingPileEncoder;
using SHWDTech.IOT.Storage.Communication.Repository;

namespace SHWD.ChargingPileBusiness
{
    public class PackageDispatcher
    {
        public static string MobileServerAddr { get; }

        private readonly MobileServerApi _requestServerApi;

        public PackageDispatcher()
        {
            _requestServerApi = new MobileServerApi(MobileServerAddr);
        }

        static PackageDispatcher()
        {
            using (var repo = new CommunicationProtocolRepository())
            {
                MobileServerAddr = repo.FindMobileServerAddrByBusinessId(ChargingPileBusinessHandler.HandledBusiness.Id);
            }

            Instance = new PackageDispatcher();
        }

        public static PackageDispatcher Instance { get; }

        public static ReceiveFeedback[] Dispatch(ChargingPileProtocolPackage package)
        {
            switch ((ProtocolCommandCategory)package.CmdType)
            {
                case ProtocolCommandCategory.SystemCommand:
                    return Instance.Response(package);
                case ProtocolCommandCategory.ConfigCommand:
                    return Instance.Response(package);
                case ProtocolCommandCategory.DataCommuinication:
                    return Instance.Receive(package);
            }

            return null;
        }

        private ReceiveFeedback[] Response(ChargingPileProtocolPackage package)
        {
            if (package.CmdByte == (int)SystemCommand.HeartBeat)
            {

            }

            return null;
        }

        private ReceiveFeedback[] Receive(ChargingPileProtocolPackage package)
        {
            var feedbacks = new ReceiveFeedback[package.PackageDataObjects.Count];
            for (var i = 0; i < feedbacks.Length; i++)
            {
                var dataObject = package.PackageDataObjects[i];
                var feedback = dataObject.Target == 0x01 
                    ? ProcessChargingPileReceive(package, dataObject) 
                    : ProcessRechargeShotReceive(package, dataObject);
                feedbacks[i] = feedback;
            }

            return feedbacks;
        }

        private ReceiveFeedback ProcessChargingPileReceive(ChargingPileProtocolPackage package, ChargingPilePackageDataObject dataObject)
        {
            if (dataObject.DataContentType == (int)ChargingPileDataType.SelfTest)
            {
                var ret = _requestServerApi.ControlResultReturn(MobileServerApi.ResultTypeSeftTest,
                    $"{dataObject.DataBytes[1]}", package.NodeIdString, package.RequestCode);
                Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd HH:mm:ss} => self test response: {ret.Result}");
            }

            return null;
        }

        private ReceiveFeedback ProcessRechargeShotReceive(ChargingPileProtocolPackage package, ChargingPilePackageDataObject dataObject)
        {
            var shot = ClientSourceStatus.FindRechargShotByIndex(package.ClientSource.ClientIdentity,
                dataObject.Target - 2);
            Task<string> response = null;
            var responseType = string.Empty;
            switch (dataObject.DataContentType)
            {
                case (int)RechargeShotDataType.StartCharging:
                    response = _requestServerApi.ControlResultReturn(MobileServerApi.ResultTypeChargingStart,
                        $"{dataObject.DataBytes[0] == 1}", shot.IdentityCode, package.RequestCode);
                    responseType = nameof(RechargeShotDataType.StartCharging);
                    break;
                case (int)RechargeShotDataType.StopCharging:
                    response = _requestServerApi.ControlResultReturn(MobileServerApi.ResultTypeChargingStop,
                        $"{dataObject.DataBytes[0] == 6}", shot.IdentityCode, package.RequestCode);
                    responseType = nameof(RechargeShotDataType.StopCharging);
                    break;
                case (int)RechargeShotDataType.ChargingAmount:
                    response = _requestServerApi.ControlResultReturn(MobileServerApi.ResultTypeChargDatas,
                        $"{BitConverter.ToUInt32(dataObject.DataBytes, 0)}", shot.IdentityCode, package.RequestCode);
                    responseType = nameof(RechargeShotDataType.ChargingAmount);
                    break;
                case (int)RechargeShotDataType.FetchRechrogeShotQrcode:
                    var identity = ClientSourceStatus.GetChargingPileIdentityByNodeId(package.NodeIdString);
                    if (!string.IsNullOrWhiteSpace(identity))
                    {
                        var dic = new Dictionary<string, string>
                        {
                            {"ShortIdengity", shot.IdentityCode},
                            {"Qrcode", shot.Qrcode}
                        };
                        var feedback = new ReceiveFeedback
                        {
                            Action = AfterReceiveAction.ReplayWithPackage,
                            Package = new FetchQrcodeEncoder().Encode(identity, dic)
                        };
                        return feedback;
                    }
                    break;
            }
            #if DEBUG
            Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd HH:mm:ss} => rechargeShot response : type:{responseType}, result: {response?.Result}");
            #endif

            return null;
        }
    }
}
