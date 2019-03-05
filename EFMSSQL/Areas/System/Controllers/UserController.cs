using Entity;
using IServices;
using Library;
using Library.jpGrid;
using Library.Pub;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Library.Excel;

namespace EFMSSQL.Areas.System.Controllers
{
    public class UserController : BaseController
    {
        private readonly ISysUserServices _iSysUserServices;

        public UserController(ISysUserServices iSysUserServices)
        {
            _iSysUserServices = iSysUserServices;
        }

        // GET: System/User
        public ActionResult Index()
        {
            var now = DateTime.Now;
            ViewBag.startDate = DateTimeHelper.FirstDayOfMonth(now).ToString("yyyy-MM-dd");
            ViewBag.endDate = DateTimeHelper.LastDayOfMonth(now).ToString("yyyy-MM-dd");
            return View();
        }

        [ValidateInput(false)]
        public ActionResult Add(SystemUser su, string ids, string editor, FormCollection fc)
        {
            var cf = fc["editor"];
            var ds = fc["editorvalue"];
            SystemUser suk = new SystemUser();
            PubResult pr = new PubResult();
            if (IsPost())
            {
                if (!string.IsNullOrEmpty(ids))
                {
                    su.SystemUserId = ids;
                    su.ModifiedBy = AuthTicket().SystemUserId;
                    su.ModifyTime = DateTime.Now;
                    var entity = _iSysUserServices.Update2(su);
                    if (entity)
                    {
                        //string js = @"<script>layer.alert('修改成功',{icon: 2,skin: 'layer-ext-moon'});setTimeout(function(){parent.location.reload();layer_close()}, 1000)</script>";
                        ViewBag.js = JsMsgSuccess("修改成功", 1);
                    }
                    else
                    {
                        ViewBag.js = JsMsgFailed("修改失败", 2);
                    }
                }
                else
                {
                    su.SystemUserId = Guid.NewGuid().ToString().Replace("-", "");
                    su.IsActive = 1;
                    var flag = _iSysUserServices.InsertAdd(su);
                    if (flag)
                    {
                        ViewBag.js = JsMsgSuccess(PubConst.Success2, 1);
                    }
                    else
                    {
                        ViewBag.js = JsMsgFailed(PubConst.Failed6, 2);
                    }
                }
                return View(su);
            }
            else
            {
                if (ids != null)
                {
                    su = _iSysUserServices.FindEntity(c => c.SystemUserId == ids);
                }
            }
            return View(su);
        }

        [HttpPost]
        public ActionResult AddOrUpdate(SystemUser su, string id)
        {
            PubResult pr = new PubResult();
            if (id != null)
            {
                var entity = _iSysUserServices.FindEntity(c => c.SystemUserId == id);
                return View("~/Areas/System/Views/User/Add.cshtml"); ;
            }
            else
            {
                su.SystemUserId = Guid.NewGuid().ToString().Replace("-", "");
                var flag = _iSysUserServices.InsertAdd(su);
                if (flag == true)
                {
                    pr.Flag = true;
                    pr.Msg = PubConst.Success2;
                }
                else
                {
                    pr.Flag = false;
                    pr.Msg = PubConst.Failed6;
                }
            }
            return Json(pr, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetUserList(JqGridModel.JqGrid jq, PubPage fg)
        {
            if (jq.page == 0)
            {
                jq.page = 1;
                jq.rows = 10;
                jq.sidx = "CreateTime";
                jq.sord = "DESC";
            }
            var list2 = _iSysUserServices.PageSystemUsers(fg, jq);
            return Json(JqGridModel.GridData(list2.Item1, jq, list2.Item2), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(string ids)
        {
            PubResult pr = new PubResult();
            if (string.IsNullOrEmpty(ids))
            {
                pr.Flag = false;
                pr.Msg = PubConst.Delete2;
            }
            else if (ids.Length > 40)
            {
                pr.Flag = false;
                pr.Msg = PubConst.Delete3;
            }
            else
            {
                ids = ids.TrimEnd(',');
                var flag = _iSysUserServices.Delete(c => c.SystemUserId == ids);
                if (flag)
                {
                    pr.Flag = true;
                    pr.Msg = PubConst.Delete1;
                }
                else
                {
                    pr.Flag = false;
                    pr.Msg = PubConst.Delete4;
                }
            }
            return Json(pr, JsonRequestBehavior.AllowGet);
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
                string sql = "delete from SystemUser where SystemUserId in (" + ids + ")" + "and 1=@shu";
                var flag = _iSysUserServices.ExecuteSqlCommand(ids, sql, new SqlParameter[] { new SqlParameter("@shu", "1") });
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

        public ActionResult RenderToExcel(PubPage fg)
        {
            string sql = "select Telephone,Pwd from SystemUser";
            var dt = _iSysUserServices.ToDataTable(sql, null);
            string[] Telephone = { "手机号", "密码" };
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt.Columns[i].ColumnName = Telephone[i];
            }
            var ms = NpoiHelper.RenderToExcel(dt);
            var xlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var xls = "application/vnd.ms-excel";
            return File(ms, xls, "用户信息" + DateTime.Now.ToString("yyyy-MM-dd") + ".xls");
        }

        public ActionResult RenderToExcel3(PubPage fg)
        {
            string sql = "select * from SystemUser";
            var dt = _iSysUserServices.ToDataTable(sql, null);
            AsposeExcel.ToExcel(dt);
            return Content("");
        }

        public ActionResult RenderToExcel2(PubPage fg)
        {
            string sql = "select * from SystemUser";
            var dt = _iSysUserServices.ToDataTable(sql, null);
            NpoiHelper.ExportExcel(dt, "用户信息" + DateTime.Now.ToString("yyyy-MM-dd") + ".xls");
            return Content("");
            //return File("application/vnd.ms-excel", "用户信息" + DateTime.Now.ToString("yyyy-MM-dd") + ".xls");
        }

        public ActionResult FileUpload()
        {
            return View();
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <returns></returns>
        public ActionResult GetFile()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "upload/";
            string fileName = "用户信息.xls";
            path = path + fileName;
            return File(new FileStream(path, FileMode.Open), "text/plain", fileName);
        }

        public ActionResult Excel()
        {
            //HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase file = Request.Files["pic"];
            if (string.IsNullOrEmpty(file.FileName))
            {
                ViewBag.error = "文件不能为空";
                return View("~/Areas/System/Views/User/FileUpload.cshtml");
            }
            else
            {
                string FileName;
                string savePath;
                string filename = Path.GetFileName(file.FileName);
                int filesize = file.ContentLength;//获取上传文件的大小单位为字节byte
                string fileEx = Path.GetExtension(filename);//获取上传文件的扩展名
                string NoFileName = Path.GetFileNameWithoutExtension(filename);//获取无扩展名的文件名
                int Maxsize = 4000 * 1024;//定义上传文件的最大空间大小为4M
                string FileType = ".xls,.xlsx";//定义上传文件的类型字符串

                FileName = NoFileName + DateTime.Now.ToString("yyyyMMddhhmmss") + fileEx;
                if (!FileType.Contains(fileEx))
                {
                    ViewBag.error = "文件类型不对，只能导入xls和xlsx格式的文件";
                    return View("~/Areas/System/Views/User/FileUpload.cshtml");
                }
                string path = AppDomain.CurrentDomain.BaseDirectory + "upload";
                savePath = Path.Combine(path, FileName);
                file.SaveAs(savePath);
                if (FileHelper.Exists(savePath))
                {
                    //导入逻辑
                    var dtt = NpoiUntil.ImportExcelAllToDt(savePath);
                    //删除
                    FileHelper.Delete(savePath);
                    ViewBag.error = "Excel导入成功";
                }
                else
                {
                    ViewBag.error = "Excel导入成功";
                }
            }
            return View("~/Areas/System/Views/User/FileUpload.cshtml");
        }
    }
}