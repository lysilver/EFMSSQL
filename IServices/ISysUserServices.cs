using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Entity;
using Library.jpGrid;
using Library.Pub;
using ViewModel;

namespace IServices
{
    public interface ISysUserServices : IBaseServices<SystemUser>
    {
        PubResult CheckLogin(string tel, string pwd);

        SystemUser GetByAccount(string account);

        Tuple<int, List<SystemUser>> PageSystemUsers(PubPage pg, JqGridModel.JqGrid jq);
    }
}