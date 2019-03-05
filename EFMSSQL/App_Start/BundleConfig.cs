using System.Web.Optimization;

namespace EFMSSQL
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.Bundles.IgnoreList.Clear();
            //AddDefaultIgnorePatterns(bundles.IgnoreList);
            //cdn
            bundles.UseCdn = true;  //开启支持cdn
            var bootstrapcdncss = "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/css/bootstrap.min.css";
            var bootstrapcdnjs = "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/js/bootstrap.min.js";
            bundles.Add(new StyleBundle("~/Content/bootstrapcdncss", bootstrapcdncss).Include(
                "~/lib/jqGrid/bootstrap.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapcdnjs", bootstrapcdnjs).Include(
                "~/lib/jqGrid/bootstrap.min.js"));
            //cdn

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                     "~/lib/jquery/1.9.1/jquery.min.js",
                     "~/static/h-ui/js/H-ui.min.js",
                     "~/static/h-ui.admin/js/H-ui.admin.js"
                     ));
            //list js页面
            bundles.Add(new ScriptBundle("~/bundles/listjs").Include(
                "~/lib/jquery/1.9.1/jquery.min.js",
                "~/static/h-ui/js/H-ui.min.js",
                "~/static/h-ui.admin/js/H-ui.admin.js",
                "~/lib/jqGrid/jquery.jqGrid.min.js",
                "~/lib/jqGrid/grid.locale-cn.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/ueditor").Include(
                "~/Content/ueditor1_4_3_3-utf8-net/ueditor.config.js",
                "~/Content/ueditor1_4_3_3-utf8-net/ueditor.all.min.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/layer").Include(
                    "~/lib/layer/layer.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqGrid").Include(
             "~/lib/jqGrid/jquery.jqGrid.js"));

            bundles.Add(new ScriptBundle("~/bundles/My97").Include(
                "~/lib/My97DatePicker/4.8/WdatePicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                "~/lib/datatables/1.10.0/jquery.dataTables.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/laypage").Include(
                "~/lib/laypage/1.2/laypage.js"));

            bundles.Add(new ScriptBundle("~/bundles/md5").Include(
                "~/Scripts/jquery.md5.js"));

            bundles.Add(new ScriptBundle("~/bundles/menu").Include(
                "~/lib/jquery.contextmenu/jquery.contextmenu.r2.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/logincss").Include(
                      "~/static/h-ui/css/H-ui.min.css", new CssRewriteUrlTransformWrapper()).Include(
                      "~/static/h-ui.admin/css/H-ui.login.css", new CssRewriteUrlTransformWrapper()).Include(
                      "~/static/h-ui.admin/css/style.css", new CssRewriteUrlTransformWrapper())
                      );

            //list css
            bundles.Add(new StyleBundle("~/Content/listcss").Include(
                    "~/static/h-ui/css/H-ui.min.css", new CssRewriteUrlTransformWrapper()).Include(
                    "~/static/h-ui.admin/css/H-ui.admin.css", new CssRewriteUrlTransformWrapper()).Include(
                    "~/static/h-ui.admin/css/style.css", new CssRewriteUrlTransformWrapper()).Include(
                    "~/static/h-ui.admin/skin/default/skin.css", new CssRewriteUrlTransformWrapper()
                ));

            bundles.Add(new StyleBundle("~/Content/fontcss").Include("~/lib/Hui-iconfont/1.0.8/iconfont.css",
                new CssRewriteUrlTransformWrapper()));

            bundles.Add(new StyleBundle("~/Content/skincss").Include("~/static/h-ui.admin/skin/default/skin.csss",
                new CssRewriteUrlTransformWrapper()));

            BundleTable.EnableOptimizations = true;
        }

        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList != null)
            {
                ignoreList.Ignore("*.intellisense.js");
                ignoreList.Ignore("*-vsdoc.js");
                ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
                ignoreList.Ignore("*.min.js", OptimizationMode.WhenEnabled);
                ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
            }
        }
    }
}