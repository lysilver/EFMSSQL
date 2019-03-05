using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using HtmlAgilityPack;
using IServices;
using Library;
using Library.jpGrid;
using Library.Pub;
using ORM;

namespace EFMSSQL.Areas.System.Controllers
{
	public class LogController : BaseController
	{
		private readonly ISysLogServices _iSysLogServices;

		public LogController(ISysLogServices iSysLogServices)
		{
			_iSysLogServices = iSysLogServices;
		}

		// GET: System/Log
		public ActionResult Index()
		{
			ViewBag.Title = "日志管理";
			var now = DateTime.Now;
			ViewBag.startDate = DateTimeHelper.FirstDayOfMonth(now).ToString("yyyy-MM-dd");
			//ViewBag.endDate = DateTimeHelper.LastDayOfMonth(now).ToString("yyyy-MM-dd");
			ViewBag.endDate = DateTime.Now.ToString("yyyy-MM-dd");

			HtmlWeb web = new HtmlWeb();
			HtmlDocument doc = web.Load("https://bing.ioliu.cn/?p=1");
			HtmlNodeCollection hrefList = doc.DocumentNode.SelectNodes("./img[@src]");
			if (hrefList != null)
			{
				foreach (HtmlNode href in hrefList)
				{
					HtmlAttribute att = href.Attributes["href"];
					string ss = att.Value;
				}
			}

			return View();
		}

		public ActionResult Index2()
		{
			ViewBag.Title = "日志管理";
			var now = DateTime.Now;
			ViewBag.startDate = DateTimeHelper.FirstDayOfMonth(now).ToString("yyyy-MM-dd");
			//ViewBag.endDate = DateTimeHelper.LastDayOfMonth(now).ToString("yyyy-MM-dd");
			ViewBag.endDate = DateTime.Now.ToString("yyyy-MM-dd");
			return View();
		}

		[HttpPost]
		public ActionResult GetLogList(JqGridModel.JqGrid jq, PubPage fg)
		{
			if (jq.page == 0)
			{
				jq.page = 1;
				jq.rows = 10;
				jq.sidx = "CreateTime";
				jq.sord = "DESC";
			}
			var list = _iSysLogServices.PageSysLog(fg, jq);
			return Json(JqGridModel.GridData(list.Item1, jq, list.Item2), JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult GetLogList2(Bootstrap.BootstrapParams bbp, PubPage pg, string strParentID)
		{
			//var db = ContextFactory.GetCurrentContext();
			//var queryable = db.Set<SysLog>().Where(c => c.CreateTime < DateTime.Now);
			//int total = queryable.Count();
			//var list = queryable.OrderByz(bbp.sort)
			//     .Skip(bbp.offset).Take(bbp.limit).ToList();
			//bbp.order = "DESC";
			//bbp.sort = "CreateTime";
			//bbp.offset = 10;
			//bbp.limit = 10;
			var list = _iSysLogServices.PageBootstrap(pg, bbp);
			return Json(Bootstrap.GridData(list.Item1, list.Item2), JsonRequestBehavior.AllowGet);
		}

		public ActionResult DeleteAll(string ids)
		{
			PubResult pr = new PubResult();
			if (string.IsNullOrEmpty(ids))
			{
				pr.Flag = false;
				pr.Msg = PubConst.Delete2;
			}
			else
			{
				ids = ids.TrimEnd(',');
				string sql = "delete from SysLog where LogId in (" + ids + ")" + "and 1=@shu";
				var flag = _iSysLogServices.ExecuteSqlCommand(ids, sql, new SqlParameter[] { new SqlParameter("@shu", "1") });
				if (flag)
				{
					pr.Flag = true;
					pr.Msg = PubConst.Delete1;
				}
				else
				{
					pr.Flag = false;
					pr.Msg = PubConst.Delete1;
				}
			}
			return Json(pr, JsonRequestBehavior.AllowGet);
		}
	}
}