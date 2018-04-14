using Cloudflow.Core.Data.Agent.Models;
using Cloudflow.Core.Extensions;
using Cloudflow.Core.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Tests.Agents
{
    public class TestAgentMonitor : IAgentMonitor
    {
        public AgentStatus LastAgentStatus { get; set; }

        public bool RunStatusChangeCaptured { get; set; }

        public bool StepOutputCaptured { get; set; }

        public void AgentStatusChanged(AgentStatus status)
        {
            Console.WriteLine($@"Agent Status Changed {status.Status}");
            LastAgentStatus = status;
        }

        public void RunStatusChanged(Run run)
        {
            Console.WriteLine($@"Run status changed {run.Status}");
            RunStatusChangeCaptured = true;
        }

        public void StepOutput(Job job, Step step, OutputEventLevels level, string message)
        {
            Console.WriteLine($@"Step output {message}");
            StepOutputCaptured = true;
        }
    }
}
