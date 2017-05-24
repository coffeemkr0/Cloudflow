using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Configuration
{
    public interface IStepConfigurationMetaData
    {
        #region Properties
        string StepName { get; }

        Type Type { get; }
        #endregion
    }
}
