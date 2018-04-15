using Cloudflow.Core.Configuration;
using Microsoft.Owin.Hosting;
using System;
using System.Configuration;
using System.IO;
using Cloudflow.Agent.Desktop.Properties;
using Cloudflow.Core.Agents;

namespace Cloudflow.Agent.Desktop
{
    internal class Program
    {
        private static readonly log4net.ILog Log = 
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main()
        {
            try
            {
                //Load the local agent configuration
                var configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    "Cloudflow", "Agent", "Agent.config");
                var agentConfigurationSettings = new AgentLocalConfigurationSettings(configFilePath);

                //Load hubs from the Agent assembly so that SignalR will pick them up
                AppDomain.CurrentDomain.Load(typeof(AgentController).Assembly.FullName);

                //Setup the SignalR messaging service first so that we can let clients know what is going on
                var url = $"http://+:{agentConfigurationSettings.Port}/CloudflowAgent/";

                Log.Info($"Starting agent host at {url}");

                var signalRHost = WebApp.Start<SignalRStartup>(url);
                Log.Info("The agent is hosted and can now be started");

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