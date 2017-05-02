using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core
{
    /// <summary>
    /// Represents what happens when a Job's trigger is fired and its steps are executed.
    /// </summary>
    public class Run
    {
        #region Properties
        public Guid Id { get; }

        public string Name { get; }

        public Job Job { get; }

        public Dictionary<string, object> Triggerdata { get; }

        public log4net.ILog RunLogger { get; }
        #endregion

        #region Constructors
        public Run(string name, Job job, Dictionary<string, object> triggerData)
        {
            this.RunLogger = log4net.LogManager.GetLogger("RunLogger." + name);

            this.Id = Guid.NewGuid();
            this.Name = name;
            this.Job = job;
            this.Triggerdata = triggerData;
        }
        #endregion

        #region Private Methods
        private void ExecuteSteps()
        {
            foreach (var step in this.Job.Steps)
            {
                this.RunLogger.Info(string.Format("Executing step {0}", step.Name));
                try
                {
                    step.Execute(this.Triggerdata);
                }
                catch (Exception ex)
                {
                    this.RunLogger.Error(ex);
                }
            }
        }
        #endregion

        #region Public Methods
        public void Start()
        {
            this.RunLogger.Info(string.Format("Starting run {0} - {1}", this.Name, this.Id));
            ExecuteSteps();
        }
        #endregion
    }
}
