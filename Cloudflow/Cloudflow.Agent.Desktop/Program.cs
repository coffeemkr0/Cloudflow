using Cloudflow.Core.Configuration;
using Cloudflow.Core.Runtime.Hubs;
using Microsoft.Owin.Hosting;
using System;
using System.Configuration;
using Cloudflow.Agent.Desktop.Properties;

namespace Cloudflow.Agent.Desktop
{
    class Program
    {
        private static readonly log4net.ILog Log = 
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

                Log.Info($"Starting agent host at {url}");

                var signalRHost = WebApp.Start<SignalRStartup>(url);
                Log.Info(string.Format("The agent is hosted and can now be started"));

                Console.WriteLine(Resources.Program_Main_Press_Ctrl_C_or_close_this_window_to_stop_the_agent_host_);
                var result = Console.ReadKey();
                while (result.Modifiers != ConsoleModifiers.Control && result.Key != ConsoleKey.C)
                {
                    result = Console.ReadKey();
                }

                signalRHost.Dispose();
            }
            catch (System.Reflection.TargetInvocationException targetInvocationEx)
            {
                Log.Warn("Could not start the agent host. Make sure that the Cloudflow.Agent.Setup program has been used to setup the agent.");
                Log.Error(targetInvocationEx);
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex);
                Console.ReadKey();
            }
        }
    }
}