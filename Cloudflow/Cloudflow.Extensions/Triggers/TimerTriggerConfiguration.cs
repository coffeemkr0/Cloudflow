
using Cloudflow.Core.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Extensions.Triggers
{
    [Export(typeof(TriggerConfiguration))]
    [ExportMetadata("TriggerName", "TimerTrigger")]
    [ExportMetadata("Type", typeof(TimerTriggerConfiguration))]
    public class TimerTriggerConfiguration : TriggerConfiguration
    {
        #region Properties
        public double Interval { get; set; }
        #endregion

        #region Constructors
        public TimerTriggerConfiguration() : base("TimerTrigger")
        {
            
        }
        #endregion
    }
}
