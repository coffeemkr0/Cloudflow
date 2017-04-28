using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core
{
    public class Job
    {
        private static readonly log4net.ILog _logger =
               log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            _logger.Info("Trigger event fired");
            ExecuteSteps(triggerData);
        }

        private void ExecuteSteps(Dictionary<string, object> triggerData)
        {
            _logger.Info("Executing steps");
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
            _logger.Info("Enabling the job");
            _logger.Debug("Initializing the trigger");
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
