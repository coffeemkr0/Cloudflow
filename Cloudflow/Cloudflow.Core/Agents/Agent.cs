using System.Collections.Generic;
using System.Reflection;
using Cloudflow.Core.Data.Agent;
using Cloudflow.Core.Data.Agent.Models;
using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.Controllers;
using log4net;

namespace Cloudflow.Core.Agents
{
    public class Agent
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IAgentMonitor _agentMonitor;
        private AgentStatus _agentStatus;
        private readonly List<IJobController> _jobControllers;

        public Agent(IAgentMonitor agentMonitor)
        {
            _agentMonitor = agentMonitor;

            AgentStatus = new AgentStatus {Status = AgentStatus.AgentStatuses.NotRunning};

            _jobControllers = GetJobControllers();
            foreach (var jobController in _jobControllers)
            {
                jobController.RunStatusChanged += JobController_RunStatusChanged;
                jobController.StepOutput += JobController_StepOutput;
            }
        }

        public AgentStatus AgentStatus
        {
            get => _agentStatus;
            set
            {
                if (_agentStatus != value)
                {
                    _agentStatus = value;
                    _agentMonitor.AgentStatusChanged(value);
                }
            }
        }

        private List<IJobController> GetJobControllers()
        {
            var jobControllers = new List<IJobController>();

            using (var agentDbContext = new AgentDbContext())
            {
                foreach (var jobDefinition in agentDbContext.JobDefinitions)
                {
                    Logger.Info($"Add job {jobDefinition.JobDefinitionId}");

                    var jobController = new JobController(jobDefinition);
                    Logger.Info($"Loading job {jobController.JobConfiguration.Name}");

                    jobControllers.Add(jobController);
                }
            }

            return jobControllers;
        }


        private void JobController_RunStatusChanged(Run run)
        {
            _agentMonitor.RunStatusChanged(run);
        }

        private void JobController_StepOutput(Job job, Step step, OutputEventLevels level, string message)
        {
            _agentMonitor.StepOutput(job, step, level, message);
        }


        public void Start()
        {
            AgentStatus = new AgentStatus {Status = AgentStatus.AgentStatuses.Starting};

            foreach (var jobController in _jobControllers) jobController.Start();

            AgentStatus = new AgentStatus {Status = AgentStatus.AgentStatuses.Running};
        }

        public void Stop()
        {
            Logger.Info("Stopping agent");

            AgentStatus = new AgentStatus {Status = AgentStatus.AgentStatuses.Stopping};

            foreach (var jobController in _jobControllers) jobController.Stop();

            Logger.Info("Waiting for any runs in progress");
            foreach (var jobController in _jobControllers) jobController.Wait();

            Logger.Info("Agent stopped");
            AgentStatus = new AgentStatus {Status = AgentStatus.AgentStatuses.NotRunning};
        }

        public List<Run> GetQueuedRuns()
        {
            var runs = new List<Run>();

            foreach (var jobController in _jobControllers) runs.AddRange(jobController.GetQueuedRuns());

            return runs;
        }
    }
}