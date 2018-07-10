using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Freelance.Startup))]
namespace Freelance
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
