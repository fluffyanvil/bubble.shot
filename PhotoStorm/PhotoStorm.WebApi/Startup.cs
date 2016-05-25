using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(PhotoStorm.WebApi.Startup))]

namespace PhotoStorm.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}",
                defaults: null
            );
            
            app.MapSignalR("/signalr/hubs", new HubConfiguration());
            app.UseWebApi(config);
        }
    }
}
