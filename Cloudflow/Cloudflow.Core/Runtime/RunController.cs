using Cloudflow.Core.Data.Agent;
using Cloudflow.Core.Data.Agent.Models;
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
        public event RunStatusChangedEventHandler RunStatusChanged;
        protected virtual void OnRunStatusChanged()
        {
            RunStatusChangedEventHandler temp = RunStatusChanged;
            if (temp != null)
            {
                temp(this.Run);
            }
        }
        #endregion

        #region Properties
        public string Name { get; }

        public Job Job { get; }

        public Dictionary<string, object> Triggerdata { get; }

        public log4net.ILog RunLogger { get; }

        public Run Run { get; set; }

        public AgentDbContext AgentDbContext { get; set; }
        #endregion

        #region Constructors
        public RunController(string name, Job job, Dictionary<string, object> triggerData)
        {
            this.RunLogger = log4net.LogManager.GetLogger("RunController." + name);

            this.Name = name;
            this.Job = job;
            this.Triggerdata = triggerData;

            this.AgentDbContext = new AgentDbContext();

            this.Run = new Run
            {
                Name = this.Name,
                JobName = this.Job.Name,
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
            foreach (var step in this.Job.Steps)
            {
                this.RunLogger.Info(string.Format("Begin step {0}", step.Name));
                try
                {
                    step.Execute(this.Triggerdata);
                    this.RunLogger.Info(string.Format("End step {0}", step.Name));
                }
                catch (Exception ex)
                {
                    this.RunLogger.Error(ex);
                    throw;
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
