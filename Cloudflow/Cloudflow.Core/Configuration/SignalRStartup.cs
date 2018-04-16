using Cloudflow.Core.Agents;
using Cloudflow.Core.Data.Agent;
using Cloudflow.Core.Extensions.Controllers;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;

namespace Cloudflow.Core.Configuration
{
    public class SignalRStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);

                var hubConfifuration = new HubConfiguration
                {
                    EnableDetailedErrors = true,
                    EnableJSONP = true
                };

                map.RunSignalR(hubConfifuration);
            });
        }
    }
}