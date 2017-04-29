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
            AppDomain.CurrentDomain.Load(typeof(GeneralMessageHub).Assembly.FullName);

            try
            {
                //Setup the SignalR messaging service first so that we can let clients know what is going on
                string url = "http://+:80/CloudflowMessaging/";
                var signalRHost = WebApp.Start<SignalRStartup>(url);
                log.Info(string.Format("Cloudflow messaging service started and running on {0}", url));

                //Setup the WCF service that does all the real work
                //Requires this admin level command on the PC that will host the service:
                //"netsh http add urlacl url=http://+:80/ServiceName user=domain\user"
                var epAddress = "http://localhost/CloudflowAgentService";
                Uri[] baseAddresses = new Uri[] { new Uri(epAddress) };
                var wcfHost = new CorsEnabledServiceHost(typeof(AgentService), baseAddresses);

                // Start listening for messages
                wcfHost.Open();

                log.Info(string.Format("Agent service started and running on {0}", epAddress));

                Console.WriteLine("Press Ctrl+C or close this window to stop the services.");
                var result = Console.ReadKey();
                while (result.Modifiers != ConsoleModifiers.Control && result.Key != ConsoleKey.C)
                {
                    result = Console.ReadKey();
                }

                signalRHost.Dispose();
                wcfHost.Close();
            }
            catch (System.Reflection.TargetInvocationException targetInvocationEx)
            {
                log.Warn("Could not start the agent. Make sure that the Cloudflow.Agent.Setup program has been used to register the needed urls.");
                log.Error(targetInvocationEx);
                Console.ReadKey();
            }
            catch (System.ServiceModel.AddressAccessDeniedException addressDeniedEx)
            {
                log.Warn("Could not start the agent. Make sure that the Cloudflow.Agent.Setup program has been used to register the needed urls.");
                log.Error(addressDeniedEx);
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