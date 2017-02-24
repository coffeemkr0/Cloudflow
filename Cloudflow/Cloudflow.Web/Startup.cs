using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cloudflow.Web.Startup))]
namespace Cloudflow.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
