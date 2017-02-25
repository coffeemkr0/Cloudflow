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
            //Requires this admin level command on PC "netsh http add urlacl url=http://+:80/Service1 user=domain\user"
            var epAddress = "http://localhost/Service1";
            Uri[] baseAddresses = new Uri[] { new Uri(epAddress) };
            using (var host = new Cloudflow.WcfServiceLibrary.CorsEnabledServiceHost(typeof(Cloudflow.WcfServiceLibrary.Service1), 
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
