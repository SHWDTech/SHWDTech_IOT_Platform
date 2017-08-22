using System.ComponentModel.DataAnnotations;

namespace ChargingPileBusiness
{
    public enum RunningStatus : ushort
    {
        [Display(Name = "未知")]
        //UnKnow Status
        Unknow = 0x00,

        [Display(Name = "在线且运行正常")]
        OnLine = 0x01,

        [Display(Name = "不在线")]
        OffLine = 0x02,

        [Display(Name = "充电桩不可用")]
        ChargingPileUnAvailable = 0x03,

        [Display(Name = "充电枪不可用")]
        RechargeShotUnAvailable = 0x04
    }
}
