using System.Web.Mvc;

namespace EFMSSQL
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CommonExceptionFilter());
        }
    }
}