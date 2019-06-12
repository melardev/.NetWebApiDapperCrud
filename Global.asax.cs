using System.Web.Http;
using WebApiDapperCrud.Seeds;

namespace WebApiDapperCrud
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            DbSeeder.Seed();
        }
    }
}
