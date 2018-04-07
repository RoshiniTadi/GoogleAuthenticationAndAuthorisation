using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GoogleAuthentication.Startup))]
namespace GoogleAuthentication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
