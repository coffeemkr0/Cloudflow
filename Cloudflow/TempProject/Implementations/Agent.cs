using System;
using System.Collections.Generic;
using TempProject.Interfaces;

namespace TempProject.Implementations
{
    public class Agent : IAgent, IJobMonitor
    {
        private IAgentMonitor _agentMonitor;
        private readonly IEnumerable<IJob> _jobs;

        public Agent(IEnumerable<IJob> jobs)
        {
            _jobs = jobs;
        }

        public void Start(IAgentMonitor agentMonitor)
        {
            _agentMonitor = agentMonitor;
            _agentMonitor.OnAgentStarted(this);

            foreach (var job in _jobs)
            {
                job.Start(this);
            }
        }

        public void Stop()
        {
            foreach (var job in _jobs)
            {
                job.Stop();
            }

            _agentMonitor.OnAgentStopped(this);
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