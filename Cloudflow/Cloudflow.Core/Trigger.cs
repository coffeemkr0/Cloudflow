using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Cloudflow.Core
{
    public class Trigger
    {
        private static readonly log4net.ILog _logger =
               log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Timer _timer;

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

        public delegate void TriggerFiredEventHandler(object sender, Dictionary<string, object> triggerData);
        public event TriggerFiredEventHandler Fired;
        protected virtual void OnFired(Dictionary<string, object> triggerData)
        {
            TriggerFiredEventHandler temp = Fired;
            if (temp != null)
            {
                this.Job.JobLogger.Info("Trigger firing");
                temp(this, triggerData);
            }
        }
        #endregion

        #region Properties
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Job Job { get; }
        #endregion

        #region Constructors
        public Trigger(Job job)
        {
            this.Id = Guid.NewGuid();
            this.Job = job;
        }
        #endregion

        #region Private Methods
        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dictionary<string, object> triggerData = new Dictionary<string, object>();
            OnFired(triggerData);
        }
        #endregion

        #region Public Methods
        public void Start()
        {
            this.Job.JobLogger.Info("Starting the trigger");

            if(_timer == null)
            {
                _timer = new Timer(3000);
                _timer.Elapsed += _timer_Elapsed;
            }
            
            _timer.Enabled = true;
        }

        public void Stop()
        {
            this.Job.JobLogger.Info("Stopping the trigger");
            _timer.Enabled = false;
        }

        public static Trigger CreateTestTrigger(Job job, string name)
        {
            Trigger trigger = new Trigger(job)
            {
                Name = name
            };
            return trigger;
        }
        #endregion
    }
}
