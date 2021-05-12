using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(Merchant.Startup))]

namespace Merchant
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
