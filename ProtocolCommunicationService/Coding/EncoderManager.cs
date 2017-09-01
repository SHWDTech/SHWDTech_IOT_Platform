using System;
using System.Collections.Generic;
using System.Linq;
using BasicUtility;
using ProtocolCommunicationService.Core;
using SHWDTech.IOT.Storage.Communication.Entities;
using SHWDTech.IOT.Storage.Communication.Repository;

namespace ProtocolCommunicationService.Coding
{
    public class EncoderManager
    {
        static EncoderManager()
        {
            ResolveBuinessHandler();
        }

        /// <summary>
        /// 系统已经加载的所有协议信息
        /// </summary>
        private static readonly List<Protocol> AllProtocols = new List<Protocol>();

        /// <summary>
        /// 系统已经加载的所有业务处理程序
        /// </summary>
        public static Dictionary<Guid, IBusinessHandler> BusinessHandlers { get; } = new Dictionary<Guid, IBusinessHandler>();

        /// <summary>
        /// 系统已经加载的所有协议解码器类实例
        /// </summary>
        private static readonly Dictionary<string, IProtocolEncoder> ProtocolEncoders = new Dictionary<string, IProtocolEncoder>();

        public static DeviceAuthenticationResult Authentication(IProtocolPackage package, Business business)
        {
            var businessControl = ServiceControl.Instance[business.Id];
            if (businessControl == null) return DeviceAuthenticationResult.Failed();

            var clientSource = (LoadExistedClientSource(package, businessControl) 
                ?? LoadDefaultClientSource(package, business)) 
                ?? LoadBusinessHandlerClientSource(package, business);

            return clientSource == null
                ? DeviceAuthenticationResult.NotRegisted()
                : DeviceAuthenticationResult.Success(clientSource);
        }

        private static IClientSource LoadExistedClientSource(IProtocolPackage package, BusinessControl conrol)
        {
            var existsIotDevice = conrol.LookUpIotDevice(package.NodeIdString);
            return existsIotDevice?.ClientSource;
        }

        private static IClientSource LoadDefaultClientSource(IProtocolPackage package, Business business)
        {
            using (var repo = new CommunicationProticolRepository())
            {
                var device = repo.FindDeviceByNodeId(business.Id, package.DeviceNodeId);
                return device != null 
                    ? new DefaultClientSource(device.DeviceName, device.NodeIdString, business) 
                    : null;
            }
        }

        private static IClientSource LoadBusinessHandlerClientSource(IProtocolPackage package, Business business)
        {
            return !BusinessHandlers.ContainsKey(business.Id) 
                ? null 
                : BusinessHandlers[business.Id].FindClientSourceByNodeId(package.NodeIdString);
        }

        public static IProtocolPackage Decode(byte[] protocolBytes)
        {
            var protocol = DetectProtocol(protocolBytes, AllProtocols);

            if (protocol == null)
            {
                return new ProtocolPackage
                {
                    Status = PackageStatus.InvalidHead
                };
            }

            var encoder = ProtocolEncoders[protocol.ProtocolModule];
            var package = encoder.Decode(protocolBytes);
            return package;
        }

        /// <summary>
        /// 读取解码器类
        /// </summary>
        /// <param name="protocols"></param>
        public static void LoadEncoder(List<Protocol> protocols)
        {
            foreach (var protocol in protocols)
            {
                try
                {
                    var encoder = UnityFactory.Resolve<IProtocolEncoder>(protocol.ProtocolModule);
                    encoder.Protocol = protocol;
                    AllProtocols.Add(protocol);
                    ProtocolEncoders.Add(protocol.ProtocolModule, encoder);
                }
                catch (Exception ex)
                {
                    LogService.Instance.Error($"Load Encoder For Protocol {protocol.ProtocolModule} Failed.", ex);
                }
            }
        }

        /// <summary>
        /// 获取所有注册的业务处理程序
        /// </summary>
        private static void ResolveBuinessHandler()
        {
            try
            {
                var handlers = UnityFactory.GetContainer().ResolveAll(typeof(IBusinessHandler));
                foreach (var handler in handlers)
                {
                    var businessHandler = (IBusinessHandler)handler;
                    BusinessHandlers.Add(businessHandler.Business.Id, businessHandler);
                    businessHandler.OnPackageDispatcher += BusinessHandlerOnOnPackageDispatcher;
                }
            }
            catch (Exception ex)
            {
                LogService.Instance.Error("Load BuinessHandler Failed.", ex);
            }
        }

        private static PackageDispatchResult BusinessHandlerOnOnPackageDispatcher(BusinessDispatchPackageEventArgs args)
        {
            var businessControl = ServiceControl.Instance[args.Business.Id];
            if (businessControl == null) return PackageDispatchResult.Failed("business service is not running");
            var device = businessControl.LookUpIotDevice(args.Package.NodeIdString);
            if (device == null) return PackageDispatchResult.Failed("device not connected");
            device.DeviceClient.Send(args.Package);

            return PackageDispatchResult.Success;
        }

        /// <summary>
        /// 检测数据对应的协议。
        /// </summary>
        /// <param name="bufferBytes"></param>
        /// <param name="protocols"></param>
        /// <returns></returns>
        private static Protocol DetectProtocol(byte[] bufferBytes, List<Protocol> protocols)
            => protocols.FirstOrDefault(obj => IsHeadMatched(bufferBytes, obj.Head));

        /// <summary>
        /// 协议帧头与字节流匹配
        /// </summary>
        /// <param name="bufferBytes">协议字节流</param>
        /// <param name="protocolHead">协议定义帧头</param>
        /// <returns>匹配返回TRUE，否则返回FALSE</returns>
        private static bool IsHeadMatched(byte[] bufferBytes, byte[] protocolHead)
        {
            if (bufferBytes.Length < protocolHead.Length)
            {
                return false;
            }

            return !protocolHead.Where((t, i) => bufferBytes[i] != t).Any();
        }
    }
}
