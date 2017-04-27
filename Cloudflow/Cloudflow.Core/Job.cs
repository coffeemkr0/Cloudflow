using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core
{
    public class Job
    {
        #region Properties
        public Trigger Trigger { get; set; }

        public List<Step> Steps { get; set; }
        #endregion

        #region Constructors
        public Job()
        {
            this.Steps = new List<Step>();
            this.Trigger = new Trigger();
        }
        #endregion

        #region Private Methods
        private void Trigger_Fired(object sender, Dictionary<string, object> triggerData)
        {
            ExecuteSteps(triggerData);
        }

        private void ExecuteSteps(Dictionary<string, object> triggerData)
        {
            foreach (var step in this.Steps)
            {
                step.Execute(triggerData);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Enables the job and initializes its trigger
        /// </summary>
        public void Enable()
        {
            this.Trigger.Fired += Trigger_Fired;
            this.Trigger.Initialize();
        }

        public static Job CreateTestJob()
        {
            Job job = new Job();

            job.Steps.Add(new Core.Step());

            return job;
        }
        #endregion
    }
}
