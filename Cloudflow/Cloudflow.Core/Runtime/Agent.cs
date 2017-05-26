using Cloudflow.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Cloudflow.Core.Data.Agent;
using Cloudflow.Core.Data.Agent.Models;
using Cloudflow.Core.Framework;
using Cloudflow.Core.Configuration;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Cloudflow.Core.Runtime
{
    public class Agent
    {
        #region Private Members
        
        #endregion

        #region Events
        public delegate void StatusChangedEventHandler(AgentStatus status);
        public event StatusChangedEventHandler StatusChanged;
        protected virtual void OnStatusChanged()
        {
            StatusChangedEventHandler temp = StatusChanged;
            if (temp != null)
            {
                temp(this.AgentStatus);
            }
        }

        public delegate void RunStatusChangedEventHandler(Run run);
        public event RunStatusChangedEventHandler RunStatusChanged;
        protected virtual void OnRunStatusChanged(Run run)
        {
            RunStatusChangedEventHandler temp = RunStatusChanged;
            if (temp != null)
            {
                temp(run);
            }
        }
        #endregion

        #region Properties
        public List<JobController> JobControllers { get; }

        public log4net.ILog AgentLogger { get; }

        private AgentStatus _agentStatus;
        public AgentStatus AgentStatus
        {
            get { return _agentStatus; }
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
            this.AgentLogger = log4net.LogManager.GetLogger("Agent." + Environment.MachineName);
            this.JobControllers = new List<JobController>();
            this.AgentStatus = new AgentStatus { Status = AgentStatus.AgentStatuses.NotRunning };
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
                    this.AgentLogger.Debug($"[Step Output] {message}");
                    break;
                case OutputEventLevels.Info:
                    this.AgentLogger.Info($"[Step Output] {message}");
                    break;
                case OutputEventLevels.Warning:
                    this.AgentLogger.Warn($"[Step Output] {message}");
                    break;
                case OutputEventLevels.Error:
                    this.AgentLogger.Error($"[Step Output] {message}");
                    break;
                case OutputEventLevels.Fatal:
                    this.AgentLogger.Fatal($"[Step Output] {message}");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region Public Methods
        public void AddJob(JobConfiguration jobConfiguration)
        {
            var jobController = new JobController(jobConfiguration);
            jobController.RunStatusChanged += JobController_RunStatusChanged;
            jobController.StepOutput += JobController_StepOutput;
            this.JobControllers.Add(jobController);
        }

        public void Start()
        {
            this.AgentLogger.Info("Starting agent");

            this.AgentStatus = new AgentStatus { Status = AgentStatus.AgentStatuses.Starting };

            foreach (var jobController in this.JobControllers)
            {
                jobController.Start();
            }

            this.AgentStatus = new AgentStatus { Status = AgentStatus.AgentStatuses.Running };
        }

        public void Stop()
        {
            this.AgentLogger.Info("Stopping agent");

            this.AgentStatus = new AgentStatus { Status = AgentStatus.AgentStatuses.Stopping };

            foreach (var jobController in this.JobControllers)
            {
                jobController.Stop();
            }

            this.AgentLogger.Info("Waiting for any runs in progress");
            foreach (var jobController in this.JobControllers)
            {
                jobController.Wait();
            }

            this.AgentLogger.Info("Agent stopped");
            this.AgentStatus = new AgentStatus { Status = AgentStatus.AgentStatuses.NotRunning };
        }

        public List<Run> GetQueuedRuns()
        {
            List<Run> runs = new List<Run>();

            foreach (var jobController in this.JobControllers)
            {
                runs.AddRange(jobController.GetQueuedRuns());
            }

            return runs;
        }

        public static Agent CreateTestAgent()
        {
            Agent agent = new Agent();

            var extensionsAssemblyPath = @"..\..\..\Cloudflow.Extensions\bin\debug\Cloudflow.Extensions.dll";

            Guid defaultJobExtensionId = Guid.Parse("62A56D5B-07E5-41A3-A637-5E7C53FCF399");
            var jobConfigurationController = new JobConfigurationController(defaultJobExtensionId, extensionsAssemblyPath);
            var jobConfiguration = jobConfigurationController.CreateNewConfiguration();
            jobConfiguration.JobName = "Hard coded test job";
            jobConfiguration.ExtensionAssemblyPath = extensionsAssemblyPath;

            var triggerExtensionId = Guid.Parse("E325CD29-053E-4422-97CF-C1C187760E88");
            var triggerConfigurationController = new TriggerConfigurationController(triggerExtensionId, extensionsAssemblyPath);
            var timerConfiguration = triggerConfigurationController.CreateNewConfiguration();
            timerConfiguration.TriggerName = "Hard coded timer trigger";
            timerConfiguration.ExtensionAssemblyPath = extensionsAssemblyPath;
            timerConfiguration.GetType().GetProperty("Interval").SetValue(timerConfiguration, 5000);
            jobConfiguration.TriggerConfigurations.Add(timerConfiguration);

            var stepExtensionId = Guid.Parse("191A3C1A-FD25-4790-8141-DFC132DA4970");
            var stepConfigurationController = new StepConfigurationController(stepExtensionId, extensionsAssemblyPath);
            var logStepConfiguration = stepConfigurationController.CreateNewConfiguration();
            logStepConfiguration.StepName = "Hard coded log step";
            logStepConfiguration.ExtensionAssemblyPath = extensionsAssemblyPath;
            logStepConfiguration.GetType().GetProperty("LogMessage").SetValue(logStepConfiguration, "Hello World!");
            jobConfiguration.StepConfigurations.Add(logStepConfiguration);

            agent.AddJob(jobConfiguration);

            return agent;
        }
        #endregion
    }
}
