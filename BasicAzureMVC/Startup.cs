using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BasicAzureMVC.Startup))]
namespace BasicAzureMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
