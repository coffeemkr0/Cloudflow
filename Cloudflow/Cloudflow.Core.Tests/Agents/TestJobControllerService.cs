using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cloudflow.Core.Agents;
using Cloudflow.Core.Extensions.Controllers;

namespace Cloudflow.Core.Tests.Agents
{
    public class TestJobControllerService : IJobControllerService
    {
        public IList<IJobController> GetJobControllers(IJobMonitor jobMonitor)
        {
            var jobControllers = new List<IJobController>
            {
                new TestJobController(jobMonitor)
            };
            return jobControllers;
        }
    }
}
