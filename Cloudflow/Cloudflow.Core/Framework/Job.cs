using Cloudflow.Core.Configuration;
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
        private CompositionContainer _container;
        #endregion

        #region Properties
        public JobConfiguration JobConfiguration { get; }

        [Import(typeof(Trigger))]
        public List<Trigger> Triggers { get; set; }

        [Import(typeof(Step))]
        public List<Step> Steps { get; set; }

        public log4net.ILog JobLogger { get; }
        #endregion

        #region Constructors
        public Job(JobConfiguration jobConfiguration)
        {
            this.JobConfiguration = jobConfiguration;
            this.JobLogger = log4net.LogManager.GetLogger($"Job.{jobConfiguration.Name}");

            this.Triggers = new List<Trigger>();
            this.Steps = new List<Step>();

            LoadConfiguration();
        }
        #endregion

        #region Private Methods
        private void LoadConfiguration()
        {
            //An aggregate catalog that combines multiple catalogs  
            var catalog = new AggregateCatalog();

            //Adds all the parts found in the Extensions assembly
            catalog.Catalogs.Add(new AssemblyCatalog(@"..\..\..\Cloudflow.Extensions\bin\debug\Cloudflow.Extensions.dll"));

            //Create the CompositionContainer with the parts in the catalog  
            _container = new CompositionContainer(catalog);

            //Fill the imports of this object  
            try
            {
                _container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                this.JobLogger.Error(compositionException);
            }

            //Convert configurations to actual objects
            foreach (var triggerConfiguration in this.JobConfiguration.TriggerConfigurations)
            {
                //For now just use a test trigger - later on, convert the trigger configuration to a trigger object using MEF
                //var trigger = new TestTrigger(triggerConfiguration);
                //trigger.Fired += Trigger_Fired;
                //this.Triggers.Add(trigger);
            }

            foreach (var stepConfiguration in this.JobConfiguration.StepConfigurations)
            {
                //For now just use a test step - later on, convert the step configuration to a step object using MEF
                //var step = new TestStep(stepConfiguration);
                //this.Steps.Add(step);
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
                foreach (var trigger in this.Triggers)
                {
                    trigger.Start();
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
                foreach (var trigger in this.Triggers)
                {
                    trigger.Stop();
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
