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
        static void Main(string[] args)
        {
            //Requires this admin level command on the PC that will host the service:
            //"netsh http add urlacl url=http://+:80/ServiceName user=domain\user"
            var epAddress = "http://localhost/CloudflowAgentService";
            Uri[] baseAddresses = new Uri[] { new Uri(epAddress) };
            using (var host = new Cloudflow.WcfServiceLibrary.CorsEnabledServiceHost(typeof(Cloudflow.WcfServiceLibrary.AgentService), 
                baseAddresses))
            {
                // Start listening for messages
                host.Open();

                Console.WriteLine("Press any key to stop the service.");
                Console.ReadKey();

                // Close the service
                host.Close();
            }
        }
    }
}