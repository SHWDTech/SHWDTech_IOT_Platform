using System.ComponentModel.DataAnnotations;

namespace ProtocolCommunicationService.Coding
{
    public enum AuthenticationStatus : byte
    {
        [Display(Name = "身份未验证")]
        UnAuthenticated = 0x00,

        [Display(Name = "身份验证失败")]
        AuthenticationFailed = 0x01,

        [Display(Name = "设备未注册")]
        DeviceNotRegisted = 0x02,

        [Display(Name = "非身份验证协议包")]
        NotAuthenticationProtocl = 0x03,

        [Display(Name = "设备加密验证失败")]
        DecryptFailed = 0x04,

        [Display(Name = "身份已验证")]
        Authenticated = 0xFF
    }
}
