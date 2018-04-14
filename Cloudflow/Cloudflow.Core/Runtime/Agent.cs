using System;
using System.Collections.Generic;
using Cloudflow.Core.Data.Agent.Models;
using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.Controllers;
using Cloudflow.Core.Data.Shared.Models;

namespace Cloudflow.Core.Runtime
{
    public class Agent
    {
        #region Private Members
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
        public List<JobController> JobControllers { get; }

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

        #region Constructors
        public Agent()
        {
            JobControllers = new List<JobController>();
            AgentStatus = new AgentStatus { Status = AgentStatus.AgentStatuses.NotRunning };
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
        public void AddJob(JobDefinition jobDefinition)
        {
            Logger.Info($"Add job {jobDefinition.JobDefinitionId}");

            var jobController = new JobController(jobDefinition);
            Logger.Info($"Loading job {jobController.JobConfiguration.Name}");

            jobController.RunStatusChanged += JobController_RunStatusChanged;
            jobController.StepOutput += JobController_StepOutput;

            JobControllers.Add(jobController);
        }

        public void Start()
        {
            AgentStatus = new AgentStatus { Status = AgentStatus.AgentStatuses.Starting };

            foreach (var jobController in JobControllers)
            {
                jobController.Start();
            }

            AgentStatus = new AgentStatus { Status = AgentStatus.AgentStatuses.Running };
        }

        public void Stop()
        {
            Logger.Info("Stopping agent");

            AgentStatus = new AgentStatus { Status = AgentStatus.AgentStatuses.Stopping };

            foreach (var jobController in JobControllers)
            {
                jobController.Stop();
            }

            Logger.Info("Waiting for any runs in progress");
            foreach (var jobController in JobControllers)
            {
                jobController.Wait();
            }

            Logger.Info("Agent stopped");
            AgentStatus = new AgentStatus { Status = AgentStatus.AgentStatuses.NotRunning };
        }

        public List<Run> GetQueuedRuns()
        {
            var runs = new List<Run>();

            foreach (var jobController in JobControllers)
            {
                runs.AddRange(jobController.GetQueuedRuns());
            }

            return runs;
        }
        #endregion
    }
}
