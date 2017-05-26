using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Framework
{
    public interface IStepMetaData
    {
        #region Properties
        string StepExtensionId { get; }
        #endregion
    }
}
