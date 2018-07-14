using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Nominas.Startup))]
namespace Nominas
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
