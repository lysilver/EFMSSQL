using System;
using System.Collections.Generic;
using System.Linq;
using Entity;
using IRepository;
using Library;
using Library.jpGrid;
using Library.Pub;
using ORM;

namespace Repository
{
    public class SysLogRepository : BaseRepository<SysLog>, ISysLogRepository
    {
        public Tuple<int, List<SysLog>> PageSysLog(PubPage pg, JqGridModel.JqGrid jq)
        {
            var queryable = BaseContext.Set<SysLog>().AsQueryable();
            if (!string.IsNullOrEmpty(pg.keyword))
            {
                queryable = queryable.Where(c => c.LogMessage == pg.keyword);
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
                return new Tuple<int, List<SysLog>>(count, list);
            }
            else
            {
                var list = queryable.OrderByDescendingz(jq.sidx).Skip(jq.rows * (jq.page - 1)).Take(jq.rows).ToList();
                return new Tuple<int, List<SysLog>>(count, list);
            }
        }

        public Tuple<int, List<SysLog>> PageBootstrap(PubPage pg, Bootstrap.BootstrapParams bbp)
        {
            var queryable = BaseContext.Set<SysLog>().AsQueryable();
            if (!string.IsNullOrEmpty(pg.keyword))
            {
                queryable = queryable.Where(c => c.LogMessage == pg.keyword);
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
            if (string.Equals(bbp.order, "ASC", StringComparison.CurrentCultureIgnoreCase))
            {
                var list = queryable.OrderByz(bbp.sort).Skip(bbp.offset).Take(bbp.limit).ToList();
                return new Tuple<int, List<SysLog>>(count, list);
            }
            else
            {
                var list = queryable.OrderByDescendingz(bbp.sort).Skip(bbp.offset).Take(bbp.limit).ToList();
                return new Tuple<int, List<SysLog>>(count, list);
            }
        }
    }
}