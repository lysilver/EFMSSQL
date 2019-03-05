using System;
using Entity;
using IServices;
using Repository;
using Services;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using ViewModel;

namespace EFMSSQL
{
    public class BaseController : Controller
    {
        private readonly ISysLogServices _sysLogServices = new SysLogServices(new SysLogRepository());

        /// <summary>
        /// 验证是否登陆
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (AuthTicket().SystemUserId == null)
            {
                var cot = filterContext.RouteData.Values["controller"].ToString();
                if (cot != "Login")
                {
                    filterContext.Result = RedirectToAction("Index", "Login", new { area = "" });
                }
            }
            base.OnActionExecuted(filterContext);
        }

        public bool IsPost()
        {
            var me = Request.HttpMethod.ToUpper();
            if (me.Equals("POST"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///前台调用 @Html.Raw(ViewBag.js),适合form提交的时候，ajax就不用了
        /// 基于layui效果弹出提示框，1s后刷新父页面并关闭该窗口
        /// </summary>
        /// <param name="id">0(!) 1(√) 2（×）3（？）4（锁）5(哭) 6(笑)</param>
        /// <returns></returns>
        [NonAction]
        public string JsAlertSuccess(string msg, int id)
        {
            string js = @"<script>layer.alert('" + msg + "',{icon:" + id + ",skin: 'layer-ext-moon'});setTimeout(function(){parent.location.reload();layer_close()}, 1000)</script>";
            return js;
        }

        [NonAction]
        public string JsAlertFailed(string msg, int id)
        {
            string js = @"<script>layer.alert('" + msg + "',{icon:" + id + ",skin: 'layer-ext-moon'});)</script>";
            return js;
        }

        [NonAction]
        public string JsMsgSuccess(string msg, int id)
        {
            string js = @"<script>layer.msg('" + msg + "',{icon:" + id + "});setTimeout(function(){parent.location.reload();layer_close()}, 1000)</script>";
            return js;
        }

        [NonAction]
        public string JsMsgFailed(string msg, int id)
        {
            string js = @"<script>layer.msg('" + msg + "',{icon:" + id + "});</script>";
            return js;
        }

        [NonAction]
        public void InsertLog(string msg, string type)
        {
            SysLog sl = new SysLog();
            sl.LogId = Guid.NewGuid().ToString().Replace("-", "");
            sl.LogMessage = msg;
            sl.LogIp = GetClientIp();
            sl.LogType = type;
            sl.LogUrl = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
            sl.CreateTime = DateTime.Now;
            sl.Creator = AuthTicket().SystemUserId;
            _sysLogServices.InsertAdd(sl);
        }

        [NonAction]
        public static string GetClientIp()
        {
            string result = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = System.Web.HttpContext.Current.Request.UserHostAddress;
            }
            return result;
        }

        /// <summary>
        /// 获取 FormsAuthenticationTicket
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public SystemUserDto AuthTicket()
        {
            SystemUserDto dto = new SystemUserDto();
            var authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                if (!string.IsNullOrEmpty(authCookie.Value))
                {
                    var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (authTicket != null)
                    {
                        dto.SystemUserId = authTicket.UserData;
                    }
                }
            }
            return dto;
        }
    }
}