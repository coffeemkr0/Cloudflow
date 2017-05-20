using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Configuration
{
    public class DefaultTriggerConfiguration : TriggerConfiguration
    {
        #region Public Methods
        public static DefaultTriggerConfiguration CreateTestTriggerConfigurtion(string name)
        {
            return new DefaultTriggerConfiguration
            {
                Name = name
            };
        }
        #endregion
    }
}
