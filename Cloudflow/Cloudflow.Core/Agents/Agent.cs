using System;
using System.Collections.Generic;
using Cloudflow.Core.Data.Agent.Models;
using Cloudflow.Core.Extensions;
using Cloudflow.Core.Jobs;

namespace Cloudflow.Core.Agents
{
    public class Agent : IAgent, IJobMonitor
    {
        private readonly IEnumerable<IJob> _jobs;
        private IAgentMonitor _agentMonitor;

        private readonly AgentStatus _agentStatus;
        public AgentStatus AgentStatus => _agentStatus;

        public Agent(IEnumerable<IJob> jobs)
        {
            _jobs = jobs;
            _agentStatus = new AgentStatus {Status = AgentStatus.AgentStatuses.NotRunning};
        }

        public void Start(IAgentMonitor agentMonitor)
        {
            _agentStatus.Status = AgentStatus.AgentStatuses.Starting;

            _agentMonitor = agentMonitor;
            _agentMonitor.OnAgentStarted(this);

            foreach (var job in _jobs) job.Start(this);

            _agentStatus.Status = AgentStatus.AgentStatuses.Running;
        }

        public void Stop()
        {
            _agentStatus.Status = AgentStatus.AgentStatuses.Stopping;

            foreach (var job in _jobs) job.Stop();

            _agentMonitor.OnAgentStopped(this);

            _agentStatus.Status = AgentStatus.AgentStatuses.NotRunning;
        }

        public void OnJobStarted(IJob job)
        {
            _agentMonitor.OnAgentActivity(this, $"[{job.GetClassName()}] Job started");
        }

        public void OnJobActivity(IJob job, string activity)
        {
            _agentMonitor.OnAgentActivity(this, $"[{job.GetClassName()}] {activity}");
        }

        public void OnJobStopped(IJob job)
        {
            _agentMonitor.OnAgentActivity(this, $"[{job.GetClassName()}] Job stopped");
        }

        public void OnException(IJob job, Exception e)
        {
            _agentMonitor.OnAgentActivity(this, $"[{job.GetClassName()}] Exception - {e}");
        }
    }
}