using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Framework
{
    public class TestJob : IJob
    {
        #region Events
        public event JobTriggerFiredEventHandler JobTriggerFired;
        protected virtual void OnTriggerFired(Trigger trigger, Dictionary<string, object> triggerData)
        {
            JobTriggerFiredEventHandler temp = JobTriggerFired;
            if (temp != null)
            {
                temp(this, trigger, triggerData);
            }
        }
        #endregion

        #region Properties
        public Guid Id { get; set; }

        public string Name { get; set; }

        private List<Trigger> _triggers;

        public List<Trigger> Triggers
        {
            get { return _triggers; }
            set
            {
                _triggers = value;
                foreach (var trigger in value)
                {
                    trigger.Fired += Trigger_Fired;
                }
            }
        }

        public List<Step> Steps { get; set; }

        public log4net.ILog JobLogger { get; }
        #endregion

        #region Constructors
        public TestJob(string name)
        {
            this.JobLogger = log4net.LogManager.GetLogger("Job." + name);

            this.Id = Guid.NewGuid();
            this.Name = name;
            this.Steps = new List<Step>();
            this.Triggers = new List<Trigger>();

            this.Steps.Add(new Step("TestStep"));

            var trigger = new Trigger("TimerTrigger");
            trigger.Fired += Trigger_Fired;
            this.Triggers.Add(trigger);
        }
        #endregion

        #region Private Methods
        private void Trigger_Fired(Trigger trigger, Dictionary<string, object> triggerData)
        {
            this.JobLogger.Info(string.Format("A trigger has fired - {0}", trigger.Name));
            OnTriggerFired(trigger, triggerData);
        }
        #endregion

        #region Public Methods
        public void Start()
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

        public void Stop()
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
