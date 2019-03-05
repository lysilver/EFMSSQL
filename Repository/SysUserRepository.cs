using System;
using System.Collections.Generic;
using System.Linq;
using Entity;
using IRepository;
using Library.jpGrid;
using Library.Pub;
using ORM;
using Library;

namespace Repository
{
    public class SysUserRepository : BaseRepository<SystemUser>, ISysUserRepository
    {
        public SystemUser GetByAccount(string account)
        {
            return base.BaseContext.Set<SystemUser>().FirstOrDefault(c => c.Telephone == account);
        }

        /// <summary>
        /// 基于jgGrid的分页
        /// </summary>
        /// <param name="pg"></param>
        /// <param name="jq"></param>
        /// <returns></returns>
        public Tuple<int, List<SystemUser>> PageSystemUsers(PubPage pg, JqGridModel.JqGrid jq)
        {
            var queryable = BaseContext.Set<SystemUser>().AsQueryable();
            if (!string.IsNullOrEmpty(pg.keyword))
            {
                queryable = queryable.Where(c => c.Telephone == pg.keyword);
            }
            if (!string.IsNullOrEmpty(pg.startDate.ToString()))
            {
                queryable = queryable.Where(c => c.CreateTime >= pg.startDate);
            }
            if (!string.IsNullOrEmpty(pg.endDate.ToString()))
            {
                var dt = DateTimeHelper.EndDateTime(pg.endDate);
                queryable = queryable.Where(c => c.CreateTime <= dt);
            }
            var count = queryable.Count();
            if (string.Equals(jq.sord, "ASC", StringComparison.CurrentCultureIgnoreCase))
            {
                var list = queryable.OrderByz(jq.sidx).Skip(jq.rows * (jq.page - 1)).Take(jq.rows).ToList();
                return new Tuple<int, List<SystemUser>>(count, list);
            }
            else
            {
                var list = queryable.OrderByDescendingz(jq.sidx).Skip(jq.rows * (jq.page - 1)).Take(jq.rows).ToList();
                return new Tuple<int, List<SystemUser>>(count, list);
            }
        }
    }
}