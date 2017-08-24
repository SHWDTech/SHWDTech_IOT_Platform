using SHWDTech.IOT.Storage.Communication.Entities;

namespace ProtocolCommunicationService.Coding
{
    public class DeviceAuthenticationResult
    {
        private static readonly DeviceAuthenticationResult FailedResult 
            = new DeviceAuthenticationResult(null, AuthenticationStatus.AuthenticationFailed);

        private static readonly DeviceAuthenticationResult NotRegistedResult
            = new DeviceAuthenticationResult(null, AuthenticationStatus.AuthenticationFailed);

        public AuthenticationStatus Status { get; }

        public Device Device { get; }

        private DeviceAuthenticationResult(Device device, AuthenticationStatus status)
        {
            Device = device;
            Status = status;
        }

        public static DeviceAuthenticationResult Failed() => FailedResult;

        public static DeviceAuthenticationResult Success(Device device)
            => new DeviceAuthenticationResult(device, AuthenticationStatus.Authenticated);

        public static DeviceAuthenticationResult NotRegisted()
            => NotRegistedResult;
    }
}
