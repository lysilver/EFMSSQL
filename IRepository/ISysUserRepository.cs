using System;
using System.Collections.Generic;
using Entity;
using Library.jpGrid;
using Library.Pub;

namespace IRepository
{
    public interface ISysLogRepository : IBaseRepository<SysLog>
    {
        Tuple<int, List<SysLog>> PageSysLog(PubPage pg, JqGridModel.JqGrid jq);

        Tuple<int, List<SysLog>> PageBootstrap(PubPage pg, Bootstrap.BootstrapParams bbp);
    }
}