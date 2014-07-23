using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Auction.Startup))]
namespace Auction
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
