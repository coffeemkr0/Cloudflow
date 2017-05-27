
using Cloudflow.Core.Configuration;
using Cloudflow.Core.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Extensions.Triggers
{
    [ExportExtension("E325CD29-053E-4422-97CF-C1C187760E88", typeof(TimerTriggerConfiguration))]
    public class TimerTriggerConfiguration : ExtensionConfiguration
    {
        #region Properties
        public double Interval { get; set; }
        #endregion
    }
}
