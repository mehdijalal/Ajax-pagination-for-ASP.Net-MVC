using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MJApp.Startup))]
namespace MJApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
