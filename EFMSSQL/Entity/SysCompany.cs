using System;

namespace Entity
{
    public class SysCompany : BaseEntity
    {
        /// <summary>
        /// Desc:主键
        /// Default:-
        /// Nullable:False
        /// </summary>
        public string CompanyId { get; set; }

        /// <summary>
        /// Desc:编号
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string CompanyNum { get; set; }

        /// <summary>
        /// Desc:公司地址
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string CompanyAddress { get; set; }

        /// <summary>
        /// Desc:法人代表
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string CompanyPerson { get; set; }

        /// <summary>
        /// Desc:开户行
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string CompanyBank { get; set; }

        /// <summary>
        /// Desc:账号
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string CompanyBankAccount { get; set; }

        /// <summary>
        /// Desc:创立时间
        /// Default:-
        /// Nullable:True
        /// </summary>
        public DateTime? CompanyBtime { get; set; }
    }
}