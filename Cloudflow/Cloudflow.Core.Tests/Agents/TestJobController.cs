using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cloudflow.Core.Agents;
using Cloudflow.Core.Data.Agent.Models;
using Cloudflow.Core.Data.Shared.Models;
using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.Controllers;

namespace Cloudflow.Core.Tests.Agents
{
    public class TestJobController : IJobController
    {
        private readonly IJobMonitor _jobMonitor;

        public bool StartCalled { get; set; }
        public bool StopCalled { get; set; }
        public bool WaitCalled { get; set; }

        public TestJobController(IJobMonitor jobMonitor)
        {
            //TODO:This is weird and hard to test
            _jobMonitor = jobMonitor;
        }

        public List<Run> GetQueuedRuns()
        {
            return new List<Run>();
        }

        public void Start()
        {
            StartCalled = true;
        }

        public void Stop()
        {
            StopCalled = true;
        }

        public void Wait()
        {
            WaitCalled = true;
        }
    }
}
