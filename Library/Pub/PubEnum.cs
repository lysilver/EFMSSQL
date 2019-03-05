using System.ComponentModel;

namespace Library.Pub
{
    public enum PubEnum
    {
        [Description("成功")]
        Success = 1,

        [Description("失败")]
        Failed = 0
    }
}