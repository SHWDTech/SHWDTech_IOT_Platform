namespace ProtocolCommunicationService.Coding
{
    public class DeviceAuthenticationResult
    {
        private static readonly DeviceAuthenticationResult FailedResult 
            = new DeviceAuthenticationResult(null, AuthenticationStatus.AuthenticationFailed);

        private static readonly DeviceAuthenticationResult NotRegistedResult
            = new DeviceAuthenticationResult(null, AuthenticationStatus.AuthenticationFailed);

        public AuthenticationStatus Status { get; }

        public IClientSource ClientSource { get; }

        private DeviceAuthenticationResult(IClientSource source, AuthenticationStatus status)
        {
            ClientSource = source;
            Status = status;
        }

        public static DeviceAuthenticationResult Failed() => FailedResult;

        public static DeviceAuthenticationResult Success(IClientSource source)
            => new DeviceAuthenticationResult(source, AuthenticationStatus.Authenticated);

        public static DeviceAuthenticationResult NotRegisted()
            => NotRegistedResult;
    }
}
