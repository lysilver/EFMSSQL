using System.ComponentModel;

namespace Library.Pub
{
    public class PubResult
    {
        [Description("标识")]
        public bool Flag { get; set; }

        [Description("信息")]
        public string Msg { get; set; }

        [Description("数据")]
        public object Data { get; set; }
    }
}