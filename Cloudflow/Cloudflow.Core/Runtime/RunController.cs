using Cloudflow.Core.Data.Agent;
using Cloudflow.Core.Data.Agent.Models;
using Cloudflow.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Runtime
{
    public class RunController
    {
        #region Events
        public delegate void RunStatusChangedEventHandler(Run run);
        public event RunStatusChangedEventHandler RunStatusChanged;
        protected virtual void OnRunStatusChanged()
        {
            RunStatusChangedEventHandler temp = RunStatusChanged;
            if (temp != null)
            {
                temp(this.Run);
            }
        }

        public delegate void RunOutputEventHandler(Run run, OutputEventLevels level, string message);
        public event RunOutputEventHandler RunOutput;
        protected virtual void OnRunOutput(OutputEventLevels level, string message)
        {
            RunOutputEventHandler temp = RunOutput;
            if (temp != null)
            {
                temp(this.Run, level, message);
            }
        }
        #endregion

        #region Properties
        public string Name { get; }

        public JobController JobController { get; }

        public Dictionary<string, object> Triggerdata { get; }

        public log4net.ILog RunLogger { get; }

        public Run Run { get; set; }

        public AgentDbContext AgentDbContext { get; set; }
        #endregion

        #region Constructors
        public RunController(string name, JobController jobController, Dictionary<string, object> triggerData)
        {
            this.RunLogger = log4net.LogManager.GetLogger("RunController." + name);

            this.Name = name;
            this.JobController = jobController;
            this.Triggerdata = triggerData;

            this.AgentDbContext = new AgentDbContext();

            this.Run = new Run
            {
                Name = this.Name,
                JobName = this.JobController.JobConfiguration.Name,
                DateQueued = DateTime.Now,
                Status = Run.RunStatuses.Queued
            };
            this.AgentDbContext.Runs.Add(this.Run);
            this.AgentDbContext.SaveChanges();

            OnRunStatusChanged();
        }
        #endregion

        #region Private Methods
        private void ExecuteSteps()
        {
            foreach (var stepController in this.JobController.StepControllers)
            {
                this.RunLogger.Info(string.Format("Begin step {0}", stepController.StepConfiguration.Name));

                try
                {
                    OnRunOutput(OutputEventLevels.Info, $"Execute step {stepController.StepConfiguration.Name}");

                    stepController.Execute();

                    this.RunLogger.Info(string.Format("End step {0}", stepController.StepConfiguration.Name));
                }
                catch (Exception ex)
                {
                    this.RunLogger.Error(ex);
                    OnRunOutput(OutputEventLevels.Error, ex.ToString());
                }
            }
        }
        #endregion

        #region Public Methods
        public void ExecuteRun()
        {
            this.RunLogger.Info(string.Format("Starting run {0}", this.Name));

            this.Run.DateStarted = DateTime.Now;
            this.Run.Status = Run.RunStatuses.Running;
            this.AgentDbContext.SaveChanges();
            OnRunStatusChanged();

            try
            {
                ExecuteSteps();
                this.Run.Status = Run.RunStatuses.Completed;
            }
            catch (Exception ex)
            {
                this.Run.Status = Run.RunStatuses.Failed;
                this.RunLogger.Error(ex);
            }

            this.Run.DateEnded = DateTime.Now;
            this.AgentDbContext.SaveChanges();
            this.AgentDbContext.Dispose();
            OnRunStatusChanged();
        }
        #endregion
    }
}
