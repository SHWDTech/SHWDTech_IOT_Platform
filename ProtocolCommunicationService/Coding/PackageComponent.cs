using BasicUtility;

namespace ProtocolCommunicationService.Coding
{
    public class PackageComponent : IPackageComponent
    {
        public string ComponentName { get; set; }

        public string DataType { get; set; }

        public int ComponentIndex { get; set; }

        public byte ValidFlag { get; set; }

        public byte[] ComponentContent { get; set; }

        public string ComponentStringValue => ComponentContent.ToHexString();
    }
}
