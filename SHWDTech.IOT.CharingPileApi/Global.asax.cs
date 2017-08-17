using System.Web;
using System.Web.Http;

namespace SHWDTech.IOT.CharingPileApi
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
