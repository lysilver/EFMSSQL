using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Entity;
using IServices;

namespace EFMSSQL.Controllers
{
	public class LoginController : BaseController
	{
		private readonly ISysUserServices _iSysUserServices;

		public LoginController(ISysUserServices iSysUserServices)
		{
			_iSysUserServices = iSysUserServices;
		}

		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public ActionResult CheckLogin(string tel, string pwd)
		{
			var re = _iSysUserServices.CheckLogin(tel, pwd);
			InsertLog(re.Msg, "登录");
			return Json(re);
		}

		public ActionResult LoginOut()
		{
			if (User.Identity.IsAuthenticated)
			{
				FormsAuthentication.SignOut();
				var cookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
				if (cookie != null)
				{
					cookie.Expires = DateTime.Now.AddDays(-1);
					System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
				}
			}
			return RedirectToAction("Index");
		}
	}
}