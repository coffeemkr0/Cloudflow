using Cloudflow.Agent.Service;
using System;
using System.Collections.Generic;
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
                var agent = Agent.CreateTestAgent();
                agent.Start();
                Console.ReadLine();

                agent.Stop();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                log.Fatal(ex);
                Console.ReadLine();
            }
        }
    }
}
