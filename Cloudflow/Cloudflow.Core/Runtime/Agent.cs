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

        public Agent(List<JobController> jobControllers)
        {
            AgentStatus = new AgentStatus {Status = AgentStatus.AgentStatuses.NotRunning};
            _jobControllers = jobControllers;

            foreach (var jobController in jobControllers)
            {
                jobController.RunStatusChanged += JobController_RunStatusChanged;
                jobController.StepOutput += JobController_StepOutput;
            }
        }

        #endregion

        #region Private Members

        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly List<JobController> _jobControllers;

        #endregion

        #region Events

        public delegate void StatusChangedEventHandler(AgentStatus status);

        public event StatusChangedEventHandler StatusChanged;

        protected virtual void OnStatusChanged()
        {
            var temp = StatusChanged;
            temp?.Invoke(AgentStatus);
        }

        public delegate void RunStatusChangedEventHandler(Run run);

        public event RunStatusChangedEventHandler RunStatusChanged;

        protected virtual void OnRunStatusChanged(Run run)
        {
            RunStatusChanged?.Invoke(run);
        }

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
                    OnStatusChanged();
                }
            }
        }

        #endregion

        #region Private Methods

        private void JobController_RunStatusChanged(Run run)
        {
            OnRunStatusChanged(run);
        }

        private void JobController_StepOutput(Job job, Step step, OutputEventLevels level, string message)
        {
            switch (level)
            {
                case OutputEventLevels.Debug:
                    Logger.Debug($"[Step Output] {message}");
                    break;
                case OutputEventLevels.Info:
                    Logger.Info($"[Step Output] {message}");
                    break;
                case OutputEventLevels.Warning:
                    Logger.Warn($"[Step Output] {message}");
                    break;
                case OutputEventLevels.Error:
                    Logger.Error($"[Step Output] {message}");
                    break;
                case OutputEventLevels.Fatal:
                    Logger.Fatal($"[Step Output] {message}");
                    break;
                default:
                    throw new NotImplementedException();
            }
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
}