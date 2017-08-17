using System.ComponentModel.DataAnnotations;

namespace SHWDTech.IOT.Storage.Authorization
{
    public enum AudienceType : byte
    {
        [Display(Name = "未知")]
        UnKnow = 0x00,

        [Display(Name = "企业实体")]
        Entity = 0x01,

        [Display(Name = "个人用户")]
        Person = 0x02,

        [Display(Name = "API服务")]
        ApiService = 0x03
    }
}
