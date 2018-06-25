using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RSAEDU.Startup))]
namespace RSAEDU
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
