
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
    [ExportMetadata("TriggerExtensionId", "E325CD29-053E-4422-97CF-C1C187760E88")]
    [ExportMetadata("Type", typeof(TimerTriggerConfiguration))]
    public class TimerTriggerConfiguration : TriggerConfiguration
    {
        #region Properties
        public double Interval { get; set; }
        #endregion

        #region Constructors
        public TimerTriggerConfiguration() : base(Guid.Parse("E325CD29-053E-4422-97CF-C1C187760E88"))
        {
            
        }
        #endregion
    }
}
