using System;

namespace Entity
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            CreateTime = DateTime.Now;
        }

        /// <summary>
        /// Desc:-
        /// Default:(getdate())
        /// Nullable:True
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// Desc:-
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// Desc:-
        /// Default:-
        /// Nullable:True
        /// </summary>
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// Desc:-
        /// Default:-
        /// Nullable:True
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}