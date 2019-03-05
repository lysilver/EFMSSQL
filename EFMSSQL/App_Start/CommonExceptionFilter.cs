using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Entity;
using IServices;
using Repository;
using Services;
using ViewModel;

namespace EFMSSQL
{
    public class CommonExceptionFilter : HandleErrorAttribute
    {
        private readonly ISysLogServices _sysLogServices = new SysLogServices(new SysLogRepository());

        public override void OnException(ExceptionContext filterContext)
        {
            Exception error = filterContext.Exception;
            SysLog sl = new SysLog();
            sl.LogType = "异常";
            sl.LogId = Guid.NewGuid().ToString().Replace("-", "");
            sl.LogMessage = error.Message;
            sl.LogUrl = HttpContext.Current.Request.RawUrl;
            sl.CreateTime = DateTime.Now;
            sl.Creator = "system";
            var authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                if (!string.IsNullOrEmpty(authCookie.Value))
                {
                    var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (authTicket != null)
                    {
                        sl.Creator = authTicket.UserData;
                    }
                }
            }
            _sysLogServices.InsertAdd(sl);
            var view = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml",
                ViewData = { ["Error"] = "" }
            };
            // filterContext.Result = view;
            filterContext.ExceptionHandled = true;
            //filterContext.Result = new RedirectToRouteResult("Login", new RouteValueDictionary());

            //filterContext.Result = new RedirectResult("/Error/Show/");
        }
    }
}