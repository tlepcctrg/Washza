using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WashZa.Startup))]
namespace WashZa
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
