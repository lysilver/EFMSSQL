using System;

namespace Entity
{
    public class SystemUser : BaseEntity
    {
        /// <summary>
        /// Desc:主键
        /// Default:-
        /// Nullable:False
        /// </summary>
        public string SystemUserId { get; set; }

        /// <summary>
        /// Desc:手机号
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// Desc:姓名
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string SystemUserName { get; set; }

        /// <summary>
        /// Desc:昵称
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string SystemUserNickName { get; set; }

        /// <summary>
        /// Desc:密码
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// Desc:登陆时间
        /// Default:-
        /// Nullable:True
        /// </summary>
        public DateTime? LoginTime { get; set; }

        /// <summary>
        /// Desc:最后一次登陆IP
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string LoginIp { get; set; }

        /// <summary>
        /// Desc:最后一次登陆IP
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string LoginIp2 { get; set; }

        /// <summary>
        /// Desc:登陆次数
        /// Default:-
        /// Nullable:True
        /// </summary>
        public int? LoginTimes { get; set; }

        /// <summary>
        /// Desc:出生日期
        /// Default:-
        /// Nullable:True
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// Desc:年龄
        /// Default:-
        /// Nullable:True
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// Desc:-
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// Desc:WorkTime
        /// Default:-
        /// Nullable:True
        /// </summary>
        public DateTime? WorkTime { get; set; }

        /// <summary>
        /// Desc:参加本单位工作时间
        /// Default:-
        /// Nullable:True
        /// </summary>
        public DateTime? YLworkTime { get; set; }

        /// <summary>
        /// Desc:可用带薪假
        /// Default:-
        /// Nullable:True
        /// </summary>
        public int? Holiday { get; set; }

        /// <summary>
        /// Desc:负责省
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// Desc:负责市
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Desc:角色ID
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// Desc:角色名称
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Desc:微信号
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string WeChat { get; set; }

        /// <summary>
        /// Desc:是否禁止登陆
        /// Default:((1))
        /// Nullable:True
        /// </summary>
        public int? IsActive { get; set; }
    }
}