using System;
using System.Collections.Generic;
using ProtocolCommunicationService.Coding;
using SHWDTech.IOT.Storage.Communication.Entities;

namespace ProtocolCommunicationService.Core
{
    public delegate PackageDispatchResult PackageDispatchHandler(BusinessDispatchPackageEventArgs args);

    public interface IBusinessHandler
    {
        Business Business { get; }

        event PackageDispatchHandler OnPackageDispatcher;

        void OnPackageReceive(ClientDecodeSucessEventArgs args);

        IClientSource FindClientSource(IProtocolPackage package);

        void ClientAuthenticated(ClientAuthenticatedArgs args);
    }

    public class BusinessDispatchPackageEventArgs : EventArgs
    {
        public Business Business { get; }

        public BusinessDispatchPackageEventArgs(Business business)
        {
            Business = business;
        }

        public BusinessDispatchPackageEventArgs(IProtocolPackage package, Business business) : this(business)
        {
            Package = package;
        }

        public IProtocolPackage Package { get; }
    }

    public class PackageDispatchResult
    {
        public string RequestCode { get; set; }

        public PackageDispatchResult(bool result)
        {
            Successed = result;
        }

        public PackageDispatchResult(params string[] error)
        {
            Errors = error;
        }

        public static PackageDispatchResult Success => new PackageDispatchResult(true);

        public bool Successed { get; }

        public static PackageDispatchResult Failed(params string[] error)
        {
            return new PackageDispatchResult(error);
        }

        public IEnumerable<string> Errors { get; }
    }

    public class ReceiveFeedback
    {
        public AfterReceiveAction Action { get; set; }

        public IProtocolPackage Package { get; set; }
    }

    public enum AfterReceiveAction
    {
        ShoutDown = 0x00,

        ReplayWithPackage = 0x01,

        ReplayWithEmpty = 0x02
    }
}
