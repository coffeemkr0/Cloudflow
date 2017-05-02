using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core
{
    public class Job
    {
        private static readonly log4net.ILog _classLogger =
               log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Events
        public event MessageEventHandler Message;
        protected virtual void OnMessage(string message)
        {
            MessageEventHandler temp = Message;
            if (temp != null)
            {
                temp(message);
            }
        }
        #endregion

        #region Properties
        public Guid Id { get; set; }

        public string Name { get; set; }

        private Trigger _trigger;

        public Trigger Trigger
        {
            get { return _trigger; }
            set
            {
                _trigger = value;
                _trigger.Fired += _trigger_Fired;
                _trigger.Message += _trigger_Message;
            }
        }

        public List<Step> Steps { get; set; }

        public log4net.ILog JobLogger { get; }
        #endregion

        #region Constructors
        public Job(string name)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.JobLogger = log4net.LogManager.GetLogger("JobLogger." + name);

            this.Steps = new List<Step>();
        }
        #endregion

        #region Private Methods
        private void _trigger_Fired(object sender, Dictionary<string, object> triggerData)
        {
            this.JobLogger.Info("Trigger fired");
            ExecuteSteps(triggerData);
        }

        private void _trigger_Message(string message)
        {
            this.JobLogger.Info("Trigger message - " + message);
            OnMessage(message);
        }

        private void Step_Message(string message)
        {
            this.JobLogger.Info("Step message - " + message);
            OnMessage(message);
        }

        private void ExecuteSteps(Dictionary<string, object> triggerData)
        {
            foreach (var step in this.Steps)
            {
                this.JobLogger.Info(string.Format("Executing step {0}", step.Name));
                try
                {
                    step.Execute(triggerData);
                }
                catch (Exception ex)
                {
                    this.JobLogger.Error(ex);
                }
            }
        }
        #endregion

        #region Public Methods
        public void Start()
        {
            this.Trigger.Start();
        }

        public void Stop()
        {
            this.Trigger.Stop();
        }

        public void AddStep(Step step)
        {
            step.Message += Step_Message;
            this.Steps.Add(step);
        }

        public static Job CreateTestJob(string name)
        {
            var job = new Job(name);

            job.AddStep(Step.CreateTestStep(name = "-TestStep"));
            job.Trigger = Trigger.CreateTestTrigger(job, name + "-TestTrigger");

            return job;
        }
        #endregion
    }
}
