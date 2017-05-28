using Cloudflow.Core.Data.Agent;
using Cloudflow.Core.Runtime;
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
                var agent = new Agent();
                using (AgentDbContext agentDbContext = new AgentDbContext())
                {
                    foreach (var jobDefinition in agentDbContext.JobDefinitions)
                    {
                        agent.AddJob(jobDefinition);
                    }
                }

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
