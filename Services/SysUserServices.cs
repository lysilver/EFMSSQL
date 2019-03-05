using Entity;
using IRepository;
using IServices;
using Library.Pub;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Security;
using Library.jpGrid;
using Repository;
using ViewModel;
using ORM;

namespace Services
{
    public class SysUserServices : BaseServices<SystemUser>, ISysUserServices
    {
        private readonly ISysUserRepository _sysUserRepository;

        public SysUserServices(ISysUserRepository iSysUserRepository)
        {
            this._sysUserRepository = iSysUserRepository;
            base.IBaseRepository = iSysUserRepository;
        }

        public SystemUser GetByAccount(string account)
        {
            return _sysUserRepository.GetByAccount(account);
        }

        public Tuple<int, List<SystemUser>> PageSystemUsers(PubPage pg, JqGridModel.JqGrid jq)
        {
            return _sysUserRepository.PageSystemUsers(pg, jq);
        }

        public PubResult CheckLogin(string tel, string pwd)
        {
            PubResult pr = new PubResult();
            //扩展成从缓存中获取用户信息，如果缓存没有用户信息，再去服务器上查找
            var model = _sysUserRepository.FindEntity(c => c.Telephone == tel);
            if (model == null)
            {
                pr.Msg = PubConst.Failed2;
                pr.Flag = false;
            }
            else
            {
                if (model.Pwd == pwd)
                {
                    switch (model.IsActive)
                    {
                        case 1:
                            SystemUserDto sud = new SystemUserDto();
                            sud.SystemUserId = model.SystemUserId;
                            //加入cookie 或者 session

                            DateTime expiration = DateTime.Now.AddDays(7);
                            //DateTime expiration2 DateTime.Now.Add(FormsAuthentication.Timeout);
                            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(2,
                                model.Telephone,
                                DateTime.Now,
                                expiration,
                                true,
                                model.SystemUserId,
                                FormsAuthentication.FormsCookiePath
                            );
                            string cookieName = "vs2017vs2015xmltool---1wjuijs";
                            //FormsAuthentication.FormsCookieName
                            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                                FormsAuthentication.Encrypt(ticket))
                            {
                                HttpOnly = true,
                                Expires = expiration
                            };
                            HttpContext.Current.Response.Cookies.Add(cookie);
                            pr.Msg = PubConst.Success;
                            pr.Flag = true;

                            break;

                        default:
                            pr.Msg = PubConst.Failed3;
                            pr.Flag = false;
                            break;
                    }
                }
                else
                {
                    pr.Msg = PubConst.Failed4;
                    pr.Flag = false;
                }
            }
            return pr;
        }
    }
}