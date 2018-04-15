using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cloudflow.Core.Data.Agent.Models;
using Cloudflow.Core.Extensions;

namespace Cloudflow.Core.Agents
{
    public interface IJobMonitor
    {
        void RunStatusChanged(Run run);

        void StepOutput(Job job, Step step, OutputEventLevels level, string message);
    }
}
