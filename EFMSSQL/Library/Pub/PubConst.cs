using System.ComponentModel;

namespace Library.Pub
{
    /// <summary>
    /// 系统中常用且不需要改变的常量
    /// </summary>
    public class PubConst
    {
        [Description("登陆成功")]
        public static readonly string Success = "登陆成功！";

        [Description("登陆失败")]
        public static readonly string Failed1 = "登陆失败！";

        [Description("该账号不存在！或者已经禁用！")]
        public static readonly string Failed2 = "该账号不存在！或者已经禁用！";

        [Description("该账号已经禁用")]
        public static readonly string Failed3 = "该账号已经禁用！";

        [Description("密码错误")]
        public static readonly string Failed4 = "密码错误！";

        [Description("删除成功")]
        public static readonly string Delete1 = "删除成功！";

        [Description("删除主键不能为空")]
        public static readonly string Delete2 = "删除主键不能为空！";

        [Description("多条删除，请选择批量删除")]
        public static readonly string Delete3 = "多条删除，请选择批量删除！";

        [Description("删除失败")]
        public static readonly string Delete4 = "删除失败！";

        public static readonly string Success2 = "添加成功！";
        public static readonly string Success3 = "批量添加成功！";
        public static readonly string Failed5 = "批量添加添加！";
        public static readonly string Failed6 = "添加失败！";
    }
}