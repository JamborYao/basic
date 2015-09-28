using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BasicAzureWeb.Startup))]
namespace BasicAzureWeb
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
