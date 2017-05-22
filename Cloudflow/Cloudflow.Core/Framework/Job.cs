using Cloudflow.Core.Configuration;
using Cloudflow.Core.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Framework
{
    public abstract class Job
    {
        #region Events
        public delegate void TriggerFiredEventHandler(Job job, Trigger trigger, Dictionary<string, object> triggerData);
        public event TriggerFiredEventHandler TriggerFired;
        #endregion

        #region Private Members
        
        #endregion

        #region Properties
        public JobConfiguration JobConfiguration { get; }

        public List<TriggerController> TriggerControllers { get; }

        public List<StepController> StepControllers { get; }

        public log4net.ILog JobLogger { get; }
        #endregion

        #region Constructors
        public Job(JobConfiguration jobConfiguration)
        {
            this.JobConfiguration = jobConfiguration;
            this.JobLogger = log4net.LogManager.GetLogger($"Job.{jobConfiguration.Name}");

            this.TriggerControllers = new List<TriggerController>();
            this.StepControllers = new List<StepController>();

            LoadConfiguration();
        }
        #endregion

        #region Private Methods
        private void LoadConfiguration()
        {
            foreach (var triggerConfiguration in this.JobConfiguration.TriggerConfigurations)
            {
                var triggerController = new TriggerController(triggerConfiguration);
                this.TriggerControllers.Add(triggerController);
            }

            foreach (var stepConfiguration in this.JobConfiguration.StepConfigurations)
            {
                var stepController = new StepController(stepConfiguration);
                this.StepControllers.Add(stepController);
            }
        }

        private void Trigger_Fired(Trigger trigger, Dictionary<string, object> triggerData)
        {
            OnTriggerFired(trigger, triggerData);
        }

        protected virtual void OnTriggerFired(Trigger trigger, Dictionary<string, object> triggerData)
        {
            TriggerFiredEventHandler temp = TriggerFired;
            if (temp != null)
            {
                temp(this, trigger, triggerData);
            }
        }
        #endregion

        #region Public Methods
        public virtual void Start()
        {
            this.JobLogger.Info("Starting the job");
            try
            {
                foreach (var triggerController in this.TriggerControllers)
                {
                    triggerController.Start();
                }
            }
            catch (Exception ex)
            {
                this.JobLogger.Error(ex);
            }
        }

        public virtual void Stop()
        {
            this.JobLogger.Info("Stopping the job");
            try
            {
                foreach (var triggerController in this.TriggerControllers)
                {
                    triggerController.Stop();
                }
            }
            catch (Exception ex)
            {
                this.JobLogger.Error(ex);
            }
        }
        #endregion
    }
}
