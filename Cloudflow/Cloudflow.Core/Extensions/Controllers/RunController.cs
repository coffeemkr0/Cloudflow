using System;
using Cloudflow.Core.Agents;
using Cloudflow.Core.Data.Agent;
using Cloudflow.Core.Data.Agent.Models;
using log4net;

namespace Cloudflow.Core.Extensions.Controllers
{
    public class RunController
    {
        #region Constructors

        public RunController(string name, JobController jobController)
        {
            RunLogger = LogManager.GetLogger("RunController." + name);

            Name = name;
            JobController = jobController;

            AgentDbContext = new AgentDbContext();

            Run = new Run
            {
                Name = Name,
                JobName = JobController.JobConfiguration.Name,
                DateQueued = DateTime.Now,
                Status = Run.RunStatuses.Queued
            };
            AgentDbContext.Runs.Add(Run);
            AgentDbContext.SaveChanges();

            OnRunStatusChanged();
        }

        #endregion

        #region Private Methods

        private void ExecuteSteps()
        {
            foreach (var stepController in JobController.StepControllers)
            {
                RunLogger.Info(string.Format("Begin step {0}", stepController.StepConfiguration.Name));

                try
                {
                    OnRunOutput(OutputEventLevels.Info, $"Execute step {stepController.StepConfiguration.Name}");

                    stepController.Execute();

                    RunLogger.Info(string.Format("End step {0}", stepController.StepConfiguration.Name));
                }
                catch (Exception ex)
                {
                    RunLogger.Error(ex);
                    OnRunOutput(OutputEventLevels.Error, ex.ToString());
                }
            }
        }

        #endregion

        #region Public Methods

        public void ExecuteRun()
        {
            RunLogger.Info(string.Format("Starting run {0}", Name));

            Run.DateStarted = DateTime.Now;
            Run.Status = Run.RunStatuses.Running;
            AgentDbContext.SaveChanges();
            OnRunStatusChanged();

            try
            {
                ExecuteSteps();
                Run.Status = Run.RunStatuses.Completed;
            }
            catch (Exception ex)
            {
                Run.Status = Run.RunStatuses.Failed;
                RunLogger.Error(ex);
            }

            Run.DateEnded = DateTime.Now;
            AgentDbContext.SaveChanges();
            AgentDbContext.Dispose();
            OnRunStatusChanged();
        }

        #endregion

        #region Events

        public delegate void RunStatusChangedEventHandler(Run run);

        public event RunStatusChangedEventHandler RunStatusChanged;

        protected virtual void OnRunStatusChanged()
        {
            var temp = RunStatusChanged;
            if (temp != null) temp(Run);
        }

        public delegate void RunOutputEventHandler(Run run, OutputEventLevels level, string message);

        public event RunOutputEventHandler RunOutput;

        protected virtual void OnRunOutput(OutputEventLevels level, string message)
        {
            var temp = RunOutput;
            if (temp != null) temp(Run, level, message);
        }

        #endregion

        #region Properties

        public string Name { get; }

        public JobController JobController { get; }

        public ILog RunLogger { get; }

        public Run Run { get; set; }

        public AgentDbContext AgentDbContext { get; set; }

        #endregion
    }
}