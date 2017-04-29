using Cloudflow.Agent.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.ServiceHost.Desktop
{
    class Program
    {
        private static readonly log4net.ILog log = 
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            //Requires this admin level command on the PC that will host the service:
            //"netsh http add urlacl url=http://+:80/ServiceName user=domain\user"
            var epAddress = "http://localhost/CloudflowAgentService";
            Uri[] baseAddresses = new Uri[] { new Uri(epAddress) };
            using (var host = new CorsEnabledServiceHost(typeof(AgentService), 
                baseAddresses))
            {
                // Start listening for messages
                host.Open();

                log.Info("Agent host started and ready to use.");
                log.Debug("This is a debug line");
                log.Warn("This is a warning line");
                log.Error("This is an error line");
                log.Fatal("This is a fatal exception line");

                Console.WriteLine("Press any key to stop the service.");
                Console.ReadKey();

                // Close the service
                host.Close();
            }
        }
    }
}