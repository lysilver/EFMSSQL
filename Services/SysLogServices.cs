using System;
using System.Collections.Generic;
using Entity;
using IRepository;
using IServices;
using Library.jpGrid;
using Library.Pub;

namespace Services
{
	public class SysLogServices : BaseServices<SysLog>, ISysLogServices
	{
		private readonly ISysLogRepository _sysUserRepository;

		public SysLogServices(ISysLogRepository iSysLogRepository)
		{
			this._sysUserRepository = iSysLogRepository;
			base.IBaseRepository = iSysLogRepository;
		}

		public Tuple<int, List<SysLog>> PageSysLog(PubPage pg, JqGridModel.JqGrid jq)
		{
			return _sysUserRepository.PageSysLog(pg, jq);
		}

		/// <summary>
		/// 基于bootstrap实现的分页
		/// </summary>
		/// <param name="pg">查询参数</param>
		/// <param name="bbp">bootstrap table 的分页相关信息</param>
		/// <returns></returns>
		public Tuple<int, List<SysLog>> PageBootstrap(PubPage pg, Bootstrap.BootstrapParams bbp)
		{
			return _sysUserRepository.PageBootstrap(pg, bbp);
		}
	}
}