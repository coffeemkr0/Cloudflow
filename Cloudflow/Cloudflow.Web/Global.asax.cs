using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Cloudflow.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly log4net.ILog _log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            ModelBinders.Binders.DefaultBinder = new CustomModelBinder();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
