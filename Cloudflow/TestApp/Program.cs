using Cloudflow.Core.Data.Agent;
using Cloudflow.Core.Extensions.Controllers;
using Cloudflow.Core.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            try
            {
                var extensionAssemblyPath = Path.GetFullPath(@"..\..\..\Cloudflow.Extensions\bin\debug\Cloudflow.Extensions.dll");
                var extensionBrowser = new ConfigurableExtensionBrowser(extensionAssemblyPath);

                Console.WriteLine("Jobs");
                foreach (var triggerMetaData in extensionBrowser.GetJobs())
                {
                    Console.WriteLine(triggerMetaData.Type);
                }

                Console.WriteLine("Triggers");
                foreach (var triggerMetaData in extensionBrowser.GetTriggers())
                {
                    Console.WriteLine(triggerMetaData.Type);
                }

                Console.WriteLine("Steps");
                foreach (var triggerMetaData in extensionBrowser.GetSteps())
                {
                    Console.WriteLine(triggerMetaData.Type);
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex);
            }

            Console.ReadLine();
        }
    }
}
