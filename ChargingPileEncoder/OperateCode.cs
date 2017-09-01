using System.ComponentModel.DataAnnotations;

namespace SHWD.ChargingPileEncoder
{
    public class OperateCode
    {
        public OperateCode(byte code)
        {
            OperateByte = code;
            Action = (Action)((code >> 7) & 1);
            Operate = (Operate)(code & 0x0F);
        }

        public Action Action { get; }

        public Operate Operate { get; }

        public byte OperateByte { get; }
    }

    public enum Action
    {
        [Display(Name = "请求")]
        Request = 0x00,

        [Display(Name = "应答")]
        Response = 0x01
    }

    public enum Operate
    {
        [Display(Name = "写入")]
        Write = 0x01,

        [Display(Name = "读取")]
        Read = 0x02,

        [Display(Name = "控制")]
        Control = 0x03,

        [Display(Name = "自动上传")]
        AutoUpload = 0x04
    }
}
