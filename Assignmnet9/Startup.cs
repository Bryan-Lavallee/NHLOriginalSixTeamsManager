using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Assignmnet9.Startup))]
namespace Assignmnet9
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
