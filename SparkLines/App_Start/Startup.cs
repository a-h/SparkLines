using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SparkLines.Startup))]

namespace SparkLines
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
} 