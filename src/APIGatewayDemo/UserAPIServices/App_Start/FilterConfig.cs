using System.Web;
using System.Web.Mvc;
using UserAPIServices.Filters;

namespace UserAPIServices
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
