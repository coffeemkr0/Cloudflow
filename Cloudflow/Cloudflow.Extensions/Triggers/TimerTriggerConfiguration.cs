using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.ExtensionAttributes;

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
