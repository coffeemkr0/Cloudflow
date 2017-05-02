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
        Timer _timer;
        static Random _rand = new Random();

        #region Events
        public delegate void TriggerFiredEventHandler(Trigger sender, Dictionary<string, object> triggerData);
        public event TriggerFiredEventHandler Fired;
        protected virtual void OnFired(Dictionary<string, object> triggerData)
        {
            TriggerFiredEventHandler temp = Fired;
            if (temp != null)
            {
                this.TriggerLogger.Info("Trigger fired");
                temp(this, triggerData);
            }
        }
        #endregion

        #region Properties
        public Guid Id { get; }

        public string Name { get; }

        public log4net.ILog TriggerLogger { get; }
        #endregion

        #region Constructors
        public Trigger(string name)
        {
            this.TriggerLogger = log4net.LogManager.GetLogger("Trigger." + name);

            this.Id = Guid.NewGuid();
            this.Name = name;
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
            this.TriggerLogger.Info("Starting the trigger");

            if(_timer == null)
            {
                var interval = _rand.Next(1, 10);
                this.TriggerLogger.Info(string.Format("Setting timer interval to {0} seconds", interval));
                _timer = new Timer(interval * 1000);
                _timer.Elapsed += _timer_Elapsed;
            }
            
            _timer.Enabled = true;
        }

        public void Stop()
        {
            this.TriggerLogger.Info("Stopping the trigger");
            _timer.Enabled = false;
        }
        #endregion
    }
}
