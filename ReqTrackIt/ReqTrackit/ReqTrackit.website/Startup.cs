using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ReqTrackit.website.Startup))]
namespace ReqTrackit.website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
