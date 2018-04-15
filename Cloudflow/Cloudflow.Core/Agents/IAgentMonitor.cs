using Cloudflow.Core.Data.Agent.Models;
using Cloudflow.Core.Extensions;

namespace Cloudflow.Core.Agents
{
    public interface IAgentMonitor
    {
        void AgentStatusChanged(AgentStatus status);

        void RunStatusChanged(Run run);

        void StepOutput(Job job, Step step, OutputEventLevels level, string message);
    }
}