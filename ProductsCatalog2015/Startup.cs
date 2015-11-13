using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProductsCatalog2015.Startup))]
namespace ProductsCatalog2015
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
