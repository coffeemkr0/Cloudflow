using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cloudflow.Core.Agents;
using Cloudflow.Core.Data.Agent;

namespace Cloudflow.Core.Extensions.Controllers
{
    public class JobControllerService : IJobControllerService
    {
        private readonly IJobDefinitionService _jobDefinitionService;

        public JobControllerService(IJobDefinitionService jobDefinitionService)
        {
            _jobDefinitionService = jobDefinitionService;
        }

        public IList<IJobController> GetJobControllers(IJobMonitor jobMonitor)
        {
            var jobControllers = new List<IJobController>();

            foreach (var jobDefinition in _jobDefinitionService.GetJobDefinitions())
            {
                var jobController = new JobController(jobDefinition, jobMonitor);

                jobControllers.Add(jobController);
            }

            return jobControllers;
        }
    }
}
