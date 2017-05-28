using Cloudflow.Core.Configuration;
using Cloudflow.Core.Runtime.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Agent.Desktop
{
    class Program
    {
        private static readonly log4net.ILog log = 
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Configuration _agentLocalConfiguration;

        static void Main(string[] args)
        {
            try
            {
                //Load the local agent configuration
                _agentLocalConfiguration = Core.Configuration.AgentLocalConfiguration.GetConfiguration();

                //Load hubs from the Agent.Service assembly so that SignalR will pick them up
                AppDomain.CurrentDomain.Load(typeof(AgentController).Assembly.FullName);

                //Setup the SignalR messaging service first so that we can let clients know what is going on
                string url = "http://+:" + _agentLocalConfiguration.AppSettings.Settings["Port"].Value +
                    "/CloudflowAgent/";

                log.Info(string.Format("Starting agent host at {0}", url));

                var signalRHost = WebApp.Start<SignalRStartup>(url);
                log.Info(string.Format("The agent is hosted and can now be started", url));

                Console.WriteLine("Press Ctrl+C or close this window to stop the agent host.");
                var result = Console.ReadKey();
                while (result.Modifiers != ConsoleModifiers.Control && result.Key != ConsoleKey.C)
                {
                    result = Console.ReadKey();
                }

                signalRHost.Dispose();
            }
            catch (System.Reflection.TargetInvocationException targetInvocationEx)
            {
                log.Warn("Could not start the agent host. Make sure that the Cloudflow.Agent.Setup program has been used to setup the agent.");
                log.Error(targetInvocationEx);
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                log.Fatal(ex);
                Console.ReadKey();
            }
        }
    }
}