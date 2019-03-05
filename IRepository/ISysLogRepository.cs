using System;
using System.Collections.Generic;
using Entity;
using Library.jpGrid;
using Library.Pub;

namespace IRepository
{
    public interface ISysUserRepository : IBaseRepository<SystemUser>
    {
        SystemUser GetByAccount(string account);

        Tuple<int, List<SystemUser>> PageSystemUsers(PubPage pg, JqGridModel.JqGrid jq);
    }
}