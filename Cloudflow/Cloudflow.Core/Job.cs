using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core
{
    public class Job
    {
        #region Private Members
        private int _runCounter = 1;
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
            }
        }

        public List<Step> Steps { get; set; }

        public log4net.ILog JobLogger { get; }
        #endregion

        #region Constructors
        public Job(string name)
        {
            this.JobLogger = log4net.LogManager.GetLogger("JobLogger." + name);

            this.Id = Guid.NewGuid();
            this.Name = name;
            this.Steps = new List<Step>();
        }
        #endregion

        #region Private Methods
        private void _trigger_Fired(Trigger sender, Dictionary<string, object> triggerData)
        {
            this.JobLogger.Info(string.Format("A trigger has fired - {0}", sender.Name));

            Run run = new Run(string.Format("{0} Run {1}", this.Name, _runCounter++),
                this, triggerData);
            run.Start();
        }
        #endregion

        #region Public Methods
        public void Start()
        {
            this.JobLogger.Info("Starting the job");
            this.Trigger.Start();
            this.JobLogger.Info("Job started");
        }

        public void Stop()
        {
            this.JobLogger.Info("Stopping the job");
            this.Trigger.Stop();
            this.JobLogger.Info("Job stopped");
        }

        public static Job CreateTestJob(string name)
        {
            var job = new Job(name);

            job.Steps.Add(new Step("WriteToFileStep"));
            job.Trigger = new Trigger("RandomTimeIntervalTrigger");

            return job;
        }
        #endregion
    }
}
