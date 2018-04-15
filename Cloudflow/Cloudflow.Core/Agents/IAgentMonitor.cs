using Cloudflow.Core.Data.Agent.Models;
using Cloudflow.Core.Extensions;

namespace Cloudflow.Core.Agents
{
    public interface IAgentMonitor : IJobMonitor
    {
        void AgentStatusChanged(AgentStatus status);
    }
}