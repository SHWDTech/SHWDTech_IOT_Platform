using System;
using System.Collections.Generic;
using ProtocolCommunicationService.Coding;

namespace ProtocolCommunicationService.Core
{
    public delegate PackageDispatchResult PackageDispatchHandler(BusinessDispatchPackageEventArgs args);

    public interface IBusinessHandler
    {
        event PackageDispatchHandler OnPackageDispatcher;

        void OnPackageReceive(IProtocolPackage package);
    }

    public class BusinessDispatchPackageEventArgs : EventArgs
    {
        public BusinessDispatchPackageEventArgs(IProtocolPackage package)
        {
            Package = package;
        }

        public IProtocolPackage Package { get; }
    }

    public class PackageDispatchResult
    {
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

        public IEnumerable<string> Errors { get; private set; }
    }
}
