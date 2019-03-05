using Entity;
using IServices;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;

namespace EFMSSQL.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ISysUserServices _iSysUserServices;

        public HomeController(ISysUserServices iSysUserServices)
        {
            _iSysUserServices = iSysUserServices;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            var cookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Index", "Login");
        }
    }
}