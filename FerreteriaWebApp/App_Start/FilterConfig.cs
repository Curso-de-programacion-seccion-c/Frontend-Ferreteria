using System.Web;
using System.Web.Mvc;
using FerreteriaWebApp.Filters;

namespace FerreteriaWebApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthFilterAttribute());
        }
    }
}
