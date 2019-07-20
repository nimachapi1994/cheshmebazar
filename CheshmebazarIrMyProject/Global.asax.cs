using IdentitySample.Models;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace IdentitySample
{
    // Note: For instructions on enabling IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=301868
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Application["totalusers"] = 0;
            Application["onlineusers"] = 0;
        }
        protected void Session_Start()
        {
            Application["totalusers"] = (int)Application["totalusers"] + 1;
            Application["onlineusers"] = (int)Application["onlineusers"] + 1;
        }
        protected void Session_End()
        {
            Application["onlineusers"] = (int)Application["onlineusers"] - 1;
        }
        protected void Application_End()
        {
        }
    }
}
