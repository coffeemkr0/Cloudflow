using Cloudflow.Agent.Service;
using Cloudflow.Agent.Service.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
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

        static void Main(string[] args)
        {
            //Load hubs from the Agent.Service assembly so that SignalR will pick them up
            AppDomain.CurrentDomain.Load(typeof(AgentController).Assembly.FullName);

            try
            {
                //Setup the SignalR messaging service first so that we can let clients know what is going on
                string url = "http://+:80/CloudflowAgent/";
                var signalRHost = WebApp.Start<SignalRStartup>(url);
                log.Info(string.Format("Cloudflow agent started and running at {0}", url));

                Console.WriteLine("Press Ctrl+C or close this window to stop the services.");
                var result = Console.ReadKey();
                while (result.Modifiers != ConsoleModifiers.Control && result.Key != ConsoleKey.C)
                {
                    result = Console.ReadKey();
                }

                signalRHost.Dispose();
            }
            catch (System.Reflection.TargetInvocationException targetInvocationEx)
            {
                log.Warn("Could not start the agent. Make sure that the Cloudflow.Agent.Setup program has been used to register the needed urls.");
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