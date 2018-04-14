using System;
using System.Collections.Generic;
using System.Reflection;
using Cloudflow.Core.Data.Agent.Models;
using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.Controllers;
using log4net;

namespace Cloudflow.Core.Runtime
{
    public class Agent
    {
        #region Constructors

        public Agent(List<IJobController> jobControllers, IAgentNotificationService agentNotificationService)
        {
            _jobControllers = jobControllers;
            _agentNotificationService = agentNotificationService;

            AgentStatus = new AgentStatus {Status = AgentStatus.AgentStatuses.NotRunning};

            foreach (var jobController in _jobControllers)
            {
                jobController.RunStatusChanged += JobController_RunStatusChanged;
                jobController.StepOutput += JobController_StepOutput;
            }
        }

        #endregion

        #region Private Members

        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly List<IJobController> _jobControllers;
        private readonly IAgentNotificationService _agentNotificationService;

        #endregion

        #region Properties

        private AgentStatus _agentStatus;

        public AgentStatus AgentStatus
        {
            get => _agentStatus;
            set
            {
                if (_agentStatus != value)
                {
                    _agentStatus = value;
                    _agentNotificationService.AgentStatusChanged(value);
                }
            }
        }

        #endregion

        #region Private Methods

        private void JobController_RunStatusChanged(Run run)
        {
            _agentNotificationService.RunStatusChanged(run);
        }

        private void JobController_StepOutput(Job job, Step step, OutputEventLevels level, string message)
        {
            _agentNotificationService.StepOutput(job, step, level, message);
        }

        #endregion

        #region Public Methods

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

        #endregion
    }

    public interface IAgentNotificationService
    {
        void AgentStatusChanged(AgentStatus status);

        void RunStatusChanged(Run run);

        void StepOutput(Job job, Step step, OutputEventLevels level, string message);
    }
}